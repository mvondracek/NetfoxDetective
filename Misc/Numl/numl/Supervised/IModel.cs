﻿// file:	Supervised\IModel.cs
//
// summary:	Declares the IModel interface
using System;
using System.IO;
using numl.Model;
using numl.Math.LinearAlgebra;

namespace numl.Supervised
{
    /// <summary>Interface for model.</summary>
    public interface IModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        Tuple<double, double> Predict(Vector y, bool p);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="p"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Predict<T>(T o, bool p); 
        /// <summary>Predicts the given o.</summary>
        /// <param name="y">The Vector to process.</param>
        /// <returns>An object.</returns>
        double Predict(Vector y);
        /// <summary>Predicts the given o.</summary>
        /// <tparam name="T">Generic type parameter.</tparam>
        /// <param name="o">The object to process.</param>
        /// <returns>A T.</returns>
        T Predict<T>(T o);
        /// <summary>Predicts the given o.</summary>
        /// <param name="o">The object to process.</param>
        /// <returns>An object.</returns>
        object Predict(object o);
        /// <summary>Gets or sets the descriptor.</summary>
        /// <value>The descriptor.</value>
        Descriptor Descriptor { get; set; }
        /// <summary>Model persistance.</summary>
        /// <param name="file">The file to load.</param>
        void Save(string file);
        /// <summary>Saves the given stream.</summary>
        /// <param name="stream">The stream to load.</param>
        void Save(Stream stream);
        /// <summary>Loads the given stream.</summary>
        /// <param name="file">The file to load.</param>
        /// <returns>An IModel.</returns>
        IModel Load(string file);
        /// <summary>Loads the given stream.</summary>
        /// <param name="stream">The stream to load.</param>
        /// <returns>An IModel.</returns>
        IModel Load(Stream stream);
    }
}