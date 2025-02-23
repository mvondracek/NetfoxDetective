﻿// Copyright (c) 2017 Jan Pluskal
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Netfox.Framework.CaptureProcessor.L7Tracking.CustomCollections
{
    /// <summary> List of linked iterables.</summary>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    public class LinkedIterableList<T> : LinkedList<T> where T : class
    {
        /// <summary> The enumerator counter.</summary>
        private Int32 _enumeratorCounter;

        /// <summary> Dispose enumerator.</summary>
        public void DisposeEnumerator() => Interlocked.Decrement(ref this._enumeratorCounter);

        /// <summary> Gets the enumerator.</summary>
        /// <returns> The enumerator.</returns>
        public new Enumerator GetEnumerator()
        {
            Interlocked.Increment(ref this._enumeratorCounter);
            return new Enumerator(this);
        }

        /// <summary> Removes this object.</summary>
        /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
        /// <param name="item">  The item. </param>
        /// <param name="force"> (Optional) true to force. </param>
        public void Remove(LinkedListNode<T> item, Boolean force = false)
        {
            if(this._enumeratorCounter != 0 && !force) { throw new InvalidOperationException("Operation is not supported!"); }
            base.Remove(item);
        }

        /// <summary> Removes this object.</summary>
        /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
        /// <param name="item">  The item. </param>
        /// <param name="force"> (Optional) true to force. </param>
        public void Remove(T item, Boolean force = false)
        {
            if(this._enumeratorCounter != 0 && !force) { throw new InvalidOperationException("Operation is not supported!"); }
            base.Remove(item);
        }

        /// <summary> Removes the first.</summary>
        /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
        public new void RemoveFirst()
        {
            if(this._enumeratorCounter != 0) { throw new InvalidOperationException("Operation is not supported!"); }
            base.RemoveFirst();
        }

        /// <summary> Removes the last.</summary>
        /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
        public new void RemoveLast()
        {
            if(this._enumeratorCounter != 0) { throw new InvalidOperationException("Operation is not supported!"); }
            base.RemoveLast();
        }

        /// <summary> An enumerator.</summary>
        public new class Enumerator : IEnumerator<T>
        {
            /// <summary> List of empties.</summary>
            private static readonly LinkedIterableList<T> EmptyList = new LinkedIterableList<T>();

            /// <summary> List of bases.</summary>
            private readonly LinkedIterableList<T> _baseList;

            /// <summary> The current list.</summary>
            private LinkedIterableList<T> _currentList;

            /// <summary> The current list node.</summary>
            private LinkedListNode<T> _currentListNode;

            /// <summary> Constructor.</summary>
            /// <param name="currentList"> The current list. </param>
            public Enumerator(LinkedIterableList<T> currentList)
            {
                this._currentList = currentList;
                this._baseList = currentList;
            }

            /// <summary>
            ///     Performs application-defined tasks associated with freeing, releasing, or resetting
            ///     unmanaged resources.
            /// </summary>
            public void Dispose() => this._baseList.DisposeEnumerator();

            /// <summary> Gets the current.</summary>
            /// <value> The current.</value>
            Object IEnumerator.Current => this.Current;

            /// <summary> Determines if we can move next.</summary>
            /// <returns> true if it succeeds, false if it fails.</returns>
            public Boolean MoveNext()
            {
                this._currentListNode = this._currentListNode == null? this._currentList.First : this._currentListNode.Next;
                if(this._currentListNode != null) { return true; }
                this.ListEndReached();
                return false;
            }

            /// <summary> Resets this object.</summary>
            public void Reset()
            {
                this._currentListNode = null;
                this._currentList = this._baseList;
            }

            /// <summary> Gets or sets the current.</summary>
            /// <value> The current.</value>
            public T Current
            {
                get { return this._currentListNode == null? null : this._currentListNode.Value; }
                private set { this._currentListNode.Value = value; }
            }

            /// <summary> Determines if we can move previous.</summary>
            /// <returns> true if it succeeds, false if it fails.</returns>
            public Boolean MovePrevious()
            {
                var ret = this.MovePreviousWithoutBlock();
                if(!ret) { this.ListEndReached(); }
                return ret;
            }

            /// <summary> Removes the current.</summary>
            public void RemoveCurrent()
            {
                var current = this.Current;
                var current1 = this._currentListNode;
                this.MovePreviousWithoutBlock();
                // _currentList.Remove(current,true);
                this._currentList.Remove(current1, true);
            }

            /// <summary> List end reached.</summary>
            private void ListEndReached() { this._currentList = EmptyList; }

            /// <summary> Determines if we can move previous without block.</summary>
            /// <returns> true if it succeeds, false if it fails.</returns>
            private Boolean MovePreviousWithoutBlock()
            {
                this._currentListNode = this._currentListNode == null? this._currentList.Last : this._currentListNode.Previous;
                if(this._currentListNode != null) { return true; }
                return false;
            }
        }
    }
}