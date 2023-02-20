using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace HereticalSolutions.Collections.Unmanaged
{
	/// <summary>
	/// A generic array stored in the unmanaged heap
	/// Courtesy of http://JacksonDunstan.com/articles/3740
	/// Courtesy of https://github.com/bepu/bepuphysics2/blob/master/BepuUtilities/Memory/Buffer.cs
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
    public unsafe struct UnmanagedGenericArray<TValue> where TValue : unmanaged
    {
        #region Variables

        /// <summary>
        /// Pointer to the unmanaged heap memory the array is stored in
        /// </summary>
        public TValue* MemoryPointer;
        
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
        
        #endregion

        /// <summary>
        /// Create the array. Its elements are initially undefined.
        /// </summary>
        /// <param name="memoryPointer">Pointer to the unmanaged heap memory the array is stored in</param>
        /// <param name="memorySize">Unmanaged memory size in bytes</param>
        /// <param name="elementSize">The size of one element of the array in bytes</param>
        /// <param name="elementCapacity">Number of elements in the array</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnmanagedGenericArray(
            TValue* memoryPointer,
            int memorySize,
            int elementSize,
            int elementCapacity)
        {
            MemoryPointer = memoryPointer;
            
            MemorySize = memorySize;
            
            ElementSize = elementSize;
            
            ElementCapacity = elementCapacity;
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
    
        #endregion

        #region Indexers

        /// <summary>
        /// Get a pointer to an element in the array
        /// </summary>
        /// <param name="index">Index of the element to get a pointer to</param>
        public ref TValue this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return ref MemoryPointer[index];
            }
        }
        
        /// <summary>
        /// Get a pointer to an element in the array
        /// </summary>
        /// <param name="index">Index of the element to get a pointer to</param>
        public ref TValue this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return ref MemoryPointer[index];
            }
        }

        #endregion
        
        #region Get

        /// <summary>
        /// Get a pointer to an element in the array
        /// </summary>
        /// <param name="index">Index of the element to get a pointer to</param>
        /// <returns>Element in the array at specified index</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue* GetPointer(int index)
        {
            return MemoryPointer + index;
        }
        
        /// <summary>
        /// Get a pointer to an element in the array
        /// </summary>
        /// <param name="index">Index of the element to get a pointer to</param>
        /// <returns>Element in the array at specified index</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue* GetPointer(uint index)
        {
            return MemoryPointer + index;
        }
        
        #endregion

        #region IndexOf

        /// <summary>
        /// Get the element's index in the array
        /// </summary>
        /// <param name="element">Target element</param>
        /// <returns>Element index</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOfPointer(TValue* element)
        {
            var distance = element - MemoryPointer;
            
            return (int)distance;
        }

        #endregion
    }   
}