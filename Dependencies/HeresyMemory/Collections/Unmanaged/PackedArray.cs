using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace HereticalSolutions.Collections.Unmanaged
{
	/// <summary>
	/// An array stored in the unmanaged heap with Pop and Push methods
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
    public unsafe struct PackedArray
    {
        #region Variables

        /// <summary>
        /// Pointer to the unmanaged heap memory the array is stored in
        /// </summary>
        public byte* MemoryPointer;
        
        /// <summary>
        /// Unmanaged memory size in bytes
        /// </summary>
        public int MemorySize;
        
        /// <summary>
        /// The size in bytes of particular element in array
        /// </summary>
        public int ElementSize;
        
        /// <summary>
        /// The maximum amount of elements allowed in the array
        /// </summary>
        public int ElementCapacity;
        
        /// <summary>
        /// The amount of elements currently allocated in the array
        /// </summary>
        public int Count;

        #endregion

        /// <summary>
        /// Create the array. Its elements are initially undefined.
        /// </summary>
        /// <param name="memoryPointer">Pointer to the unmanaged heap memory the array is stored in</param>
        /// <param name="memorySize">Unmanaged memory size in bytes</param>
        /// <param name="elementSize">The size of one element of the array in bytes</param>
        /// <param name="elementCapacity">Number of elements in the array</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PackedArray(
            byte* memoryPointer,
            int memorySize,
            int elementSize,
            int elementCapacity)
        {
            MemoryPointer = memoryPointer;
            
            MemorySize = memorySize;
            
            ElementSize = elementSize;
            
            ElementCapacity = elementCapacity;
            
            Count = 0;
        }
        
        #region Validation

        /// <summary>
        /// Is given index valid for the array
        /// </summary>
        /// <param name="index">Target index</param>
        /// <returns>Is index valid</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IndexValid(int index)
        {
            return (index > -1) && (index < ElementCapacity);
        }

		/// <summary>
		/// Is given index valid for pushing an element
		/// </summary>
		/// <param name="index">Target index</param>
		/// <returns>Is index valid</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IndexValidForPush(int index)
        {
			return (index > -1) && (index < Count);
		}

		/// <summary>
		/// Does the array have any free elements?
		/// </summary>
		public bool HasFreeSpace
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Count < ElementCapacity;
			}
		}
    
        #endregion

        #region Indexers

        /// <summary>
        /// Get a pointer to an element in the array
        /// </summary>
        /// <param name="index">Index of the element to get a pointer to</param>
        public void* this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return MemoryPointer + ElementSize * index;
            }
        }
        
        /// <summary>
        /// Get a pointer to an element in the array
        /// </summary>
        /// <param name="index">Index of the element to get a pointer to</param>
        public void* this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return MemoryPointer + ElementSize * index;
            }
        }

        #endregion

        #region Get (generic)
        
        /// <summary>
        /// Get a pointer to an element in the array
        /// </summary>
        /// <param name="index">Index of the element to get a pointer to</param>
        /// <typeparam name="T">Element type</typeparam>
        /// <returns>Element in the array at specified index</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T* GetGeneric<T>(int index) where T : unmanaged
        {
            return (T*)(MemoryPointer + ElementSize * index);
        }
        
        /// <summary>
        /// Get a pointer to an element in the array
        /// </summary>
        /// <param name="index">Index of the element to get a pointer to</param>
        /// <typeparam name="T">Element type</typeparam>
        /// <returns>Element in the array at specified index</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T* GetGeneric<T>(uint index) where T : unmanaged
        {
            return (T*)(MemoryPointer + ElementSize * index);
        }

        #endregion

        #region Index of
        
        /// <summary>
        /// Get the element's index in the array
        /// </summary>
        /// <param name="element">Target element</param>
        /// <returns>Element index</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOfPointer(void* element)
        {
            long distance = (byte*)element - MemoryPointer;
            
            return (int)(distance / ElementSize);
        }
        
        /// <summary>
        /// Get the element's index in the array
        /// </summary>
        /// <param name="element">Target element</param>
        /// <returns>Element index</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOfGeneric<T>(T* element) where T : unmanaged
        {
            long distance = (byte*)element - MemoryPointer;
            
            return (int)(distance / ElementSize);
        }
        
        #endregion
        
        #region Pop

        /// <summary>
        /// Pop a free element from the array
        /// </summary>
        /// <returns>Free element</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void* PopPointer()
        {
            if (!HasFreeSpace)
                throw new Exception("[PackedArray] No more space");
            
            var result = this[Count];
            
            Count++;
            
            return result;
        }
        
        /// <summary>
        /// Pop a free element from the array
        /// </summary>
        /// <returns>Free element</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T* PopGeneric<T>() where T: unmanaged
        {
			//Validation checks should be performed by whoever has this struct instead

			//if (!HasFreeSpace)
			//    throw new Exception("[PackedArray] No more space");

			var result = GetGeneric<T>(Count);
            
            Count++;
            
            return result;
        }
        
        #endregion

        #region Push

        /// <summary>
        /// Push an element back to the array
        /// </summary>
        /// <param name="index">Element index</param>
        /// <returns>Index of the element that took the place of the pushed element</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int PushIndex(int index)
        {
			//Validation checks should be performed by whoever has this struct instead

			//if (index < 0
            //    || index >= Count)
            //    throw new Exception($"[PackedArray] Invalid index {index}");
            
            if (index == Count - 1)
            {
                Count--;
                
                return -1;
            }
            
            if (index != Count - 1)
                Buffer.MemoryCopy(
                    this[Count - 1],
                    this[index],
                    ElementSize,
                    ElementSize);
            
            int returnValue = Count - 1;
            
            Count--;
                
            return returnValue;
        }
        
        /// <summary>
        /// Push an element back to the array
        /// </summary>
        /// <param name="index">Element index</param>
        /// <returns>Index of the element that took the place of the pushed element</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int PushIndex(uint index)
        {
			//Validation checks should be performed by whoever has this struct instead

			//if (index < 0
            //    || index >= Count)
            //    throw new Exception($"[PackedArray] Invalid index {index}");
            
            if (index == Count - 1)
            {
                Count--;
                
                return -1;
            }
            
            if (index != Count - 1)
                Buffer.MemoryCopy(
                    this[Count - 1],
                    this[index],
                    ElementSize,
                    ElementSize);
            
            int returnValue = Count - 1;
            
            Count--;
                
            return returnValue;
        }
        
        /// <summary>
        /// Push an element back to the array
        /// </summary>
        /// <param name="element">Element to push</param>
        /// <returns>Index of the element that took the place of the pushed element</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int PushPointer(void* element)
        {
            int elementIndex = IndexOfPointer(element);
            
            return PushIndex(elementIndex);
        }
        
        /// <summary>
        /// Push an element back to the array
        /// </summary>
        /// <param name="element">Element to push</param>
        /// <typeparam name="T">Element type</typeparam>
        /// <returns>Index of the element that took the place of the pushed element</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int PushGeneric<T>(T* element) where T: unmanaged
        {
            int elementIndex = IndexOfGeneric<T>(element);
            
            return PushIndex(elementIndex);
        }

        #endregion
    }   
}