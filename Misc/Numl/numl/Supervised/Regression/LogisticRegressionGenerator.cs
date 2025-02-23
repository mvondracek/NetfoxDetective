﻿using numl.Math.LinearAlgebra;

namespace numl.Supervised.Regression
{
    /// <summary>A logistic regression generator.</summary>
    public class LogisticRegressionGenerator : Generator
    {
        /// <summary>
        /// The regularisation term Lambda
        /// </summary>
        public double Lambda { get; set; }

        /// <summary>
        /// The additional number of quadratic features to create.
        /// <para>(A higher value may overfit training data)</para>
        /// </summary>
        public int PolynomialFeatures { get; set; }

        /// <summary>Gets or sets the maximum iterations used with gradient descent.</summary>
        /// <para>The default is 500</para>
        /// <value>The maximum iterations.</value>
        public int MaxIterations { get; set; }

        /// <summary>Gets or sets the learning rate used with gradient descent.</summary>
        /// <para>The default value is 0.01</para>
        /// <value>The learning rate.</value>
        public double LearningRate { get; set; }

        /// <summary>
        /// Initialises a LogisticRegressionGenerator object
        /// </summary>
        public LogisticRegressionGenerator()
        {
            this.Lambda = 1;
            this.MaxIterations = 500;
            this.PolynomialFeatures = 5;
            this.LearningRate = 0.3;
        }

        /// <summary>Generate Logistic Regression model based on a set of examples.</summary>
        /// <param name="x">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>Model.</returns>
        public override IModel Generate(Matrix x, Vector y)
        {
            // create initial theta
            Matrix copy = x.Copy();

            copy = PreProcessing.FeatureDimensions.IncreaseDimensions(copy, this.PolynomialFeatures);

            // add intercept term
            copy = copy.Insert(Vector.Ones(copy.Rows), 0, VectorType.Col);

            Vector theta = Vector.Ones(copy.Cols);

            var run = Math.Optimization.GradientDescent.Run(theta, copy, y, this.MaxIterations, this.LearningRate, new Math.Functions.Cost.LogisticCostFunction(), 
                this.Lambda, new Math.Functions.Regularization.Regularization());

            LogisticRegressionModel model = new LogisticRegressionModel()
            {
                Descriptor = this.Descriptor,
                Theta = run.Item2,
                LogisticFunction = new Math.Functions.Logistic(),
                PolynomialFeatures = this.PolynomialFeatures
            };

            return model;
        }
    }
}
