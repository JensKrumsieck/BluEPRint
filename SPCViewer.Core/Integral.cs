﻿using ChemSharp;
using ChemSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SPCViewer.Core
{
    public class Integral : INotifyPropertyChanged
    {
        private DataPoint[] _dataPoints;

        /// <summary>
        /// Integral starting DataPoint
        /// </summary>
        public DataPoint[] DataPoints
        {
            get => _dataPoints;
            set
            {
                _dataPoints = value;
                OnPropertyChanged();
            }
        }


        private bool _editIndicator;
        /// <summary>
        /// Indicates to GUI that editing is in progress
        /// </summary>
        public bool EditIndicator
        {
            get => _editIndicator;
            set
            {
                _editIndicator = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// X Value of beginning
        /// </summary>
        public double From => DataPoints.Min(s => s.X);
        /// <summary>
        /// X Vlue of End
        /// </summary>
        public double To => DataPoints.Max(s => s.X);

        private double _value;
        /// <summary>
        /// Value of Integral
        /// </summary>
        public double Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Integrals RawValue without Factor Multiplication
        /// </summary>
        public double RawValue => DataPoints.Integrate().Last().Y;

        private double _factor = 1;
        /// <summary>
        /// Factor of Integral
        /// </summary>
        public double Factor
        {
            get => _factor; set

            {
                _factor = value;
                OnPropertyChanged();
            }
        }

        public Integral(IEnumerable<DataPoint> selection)
        {
            var dataPoints = selection as DataPoint[] ?? selection.ToArray();
            if (!dataPoints.Any()) throw new ArgumentNullException(nameof(selection));
            PropertyChanged += OnPropertyChanged;
            DataPoints = dataPoints.ToArray();
        }

        /// <summary>
        /// Recalculates Integral
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(DataPoints) && e.PropertyName != nameof(Factor)) return;
            Value = RawValue / Factor;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
