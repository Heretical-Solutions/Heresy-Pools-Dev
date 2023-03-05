using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using HereticalSolutions.Collections.Allocations;
using HereticalSolutions.Collections.Unmanaged.Internal;

namespace HereticalSolutions.Collections.Unmanaged
{
    /// <summary>
    /// A memory pool for allocating elements on the unmanaged memory buffer
    /// Courtesy of http://JacksonDunstan.com/articles/3770
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MemoryPool
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
        /// The linked list of free elements
        /// </summary>
        public MemoryPoolAllocationDescriptor* AllocationDescriptors;
        
        /// <summary>
        /// The linked list of free elements
        /// </summary>
        public MemoryPoolAllocationDescriptor* FreeList;
        
        /*
        /// <summary>
        /// The linked list of allocated elements
        /// </summary>
        public MemoryPoolAllocationDescriptor* AllocatedList;
        */
        
        #endregion

        /// <summary>
        /// Create the array. Its elements are initially undefined.
        /// </summary>
        /// <param name="memoryPointer">Pointer to the unmanaged heap memory the array is stored in</param>
        /// <param name="memorySize">Unmanaged memory size in bytes</param>
        /// <param name="elementSize">The size of one element of the array in bytes</param>
        /// <param name="elementCapacity">Number of elements in the array</param>
        /// <param name="allocationDescriptors">Array of allocation descriptors</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MemoryPool(
            byte* memoryPointer,
            int memorySize,
            int elementSize,
            int elementCapacity,
            MemoryPoolAllocationDescriptor* allocationDescriptors)
        {
            MemoryPointer = memoryPointer;
            
            MemorySize = memorySize;
            
            ElementSize = elementSize;
            
            ElementCapacity = elementCapacity;
            
            AllocationDescriptors = allocationDescriptors;
            
            FreeList = allocationDescriptors;
            
            for (int i = 0; i < ElementCapacity - 1; i++)
                (allocationDescriptors + i)->Next = (allocationDescriptors + i + 1);
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
		/// Does the array have any free elements?
		/// </summary>
		public bool HasFreeSpace
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return FreeList != null;
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

        #region Get(generic)

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
                throw new Exception("[MemoryPool] No more space");
            
            //Pop the next allocation descriptor from the free list
            var allocation = FreeList;
            
            if (allocation->Status == EAllocationStatus.USED)
                throw new Exception("[MemoryPool] Attempt to use allocation that is already in use");
            
            
            //Remove allocation descriptor from the free list and make it's next chain the current list pointer
            var nextAllocation = allocation->Next;
            
            FreeList = nextAllocation;
            
            allocation->Next = null;
            
            
            //Update allocation's status
            allocation->Status = EAllocationStatus.USED;
            
            
            //Return
            return allocation->MemoryPointer;
        }
        
        /// <summary>
        /// Pop a free element from the array
        /// </summary>
        /// <returns>Free element</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T* PopGeneric<T>() where T: unmanaged
        {
            if (!HasFreeSpace)
                throw new Exception("[MemoryPool] No more space");
            
            //Pop the next allocation descriptor from the free list
            var allocation = FreeList;
            
            if (allocation->Status == EAllocationStatus.USED)
                throw new Exception("[MemoryPool] Attempt to use allocation that is already in use");
            
            
            //Remove allocation descriptor from the free list and make it's next chain the current list pointer
            var nextAllocation = allocation->Next;
            
            FreeList = nextAllocation;
            
            allocation->Next = null;
            
            
            //Update allocation's status
            allocation->Status = EAllocationStatus.USED;
            
            
            //Return
            return (T*)allocation->MemoryPointer;
        }

        #endregion
        
        #region Push

        /// <summary>
        /// Push an element back to the array
        /// </summary>
        /// <param name="element">Element to push</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PushPointer(void* element)
        {
            int elementIndex = IndexOfPointer(element);
            
            
            //Pop the next allocation descriptor from the free list
            var allocation = AllocationDescriptors + elementIndex;
            
            if (allocation->Status == EAllocationStatus.FREE)
                return;
            
            
            //Add the allocation to the free allocations list and chain the current list head as its next element
            var nextAllocation = FreeList;
            
            allocation->Next = nextAllocation;
            
            FreeList = allocation;
            
            
            //Update allocation's status
            allocation->Status = EAllocationStatus.FREE;
        }
        
        /// <summary>
        /// Push an element back to the array
        /// </summary>
        /// <param name="element">Element to push</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PushGeneric<T>(T* element) where T: unmanaged
        {
            int elementIndex = IndexOfGeneric<T>(element);
            
            
            //Pop the next allocation descriptor from the free list
            var allocation = AllocationDescriptors + elementIndex;
            
            if (allocation->Status == EAllocationStatus.FREE)
                return;
            
            
            //Add the allocation to the free allocations list and chain the current list head as its next element
            var nextAllocation = FreeList;
            
            allocation->Next = nextAllocation;
            
            FreeList = allocation;
            
            
            //Update allocation's status
            allocation->Status = EAllocationStatus.FREE;
        }

        #endregion
    }   
}