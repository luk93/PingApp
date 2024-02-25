using PingApp.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Models
{
    public class Config : ObservableBaseModel
    {
        private int _sheetIndex;
        public int SheetIndex
        {
            get
            {
                return _sheetIndex;
            }
            set
            {
                if (_sheetIndex != value) 
                {
                    _sheetIndex = value;
                    OnPropertyChanged(nameof(SheetIndex));
                }
            }
        }
        private int _startRow;
        public int StartRow
        {
            get
            {
                return _startRow;
            }
            set
            {
                if (_startRow != value)
                {
                    _startRow = value;
                    OnPropertyChanged(nameof(StartRow));
                }
            }
        }
        private int _startColumn;
        public int StartColumn
        {
            get
            {
                return _startColumn;
            }
            set
            {
                if (_startColumn != value)
                {
                    _startColumn = value;
                    OnPropertyChanged(nameof(StartColumn));
                }
            }
        }
        private int _pingerTimeout;
        public int PingerTimeout
        {
            get
            {
                return _pingerTimeout;
            }
            set
            {
                if (_pingerTimeout != value)
                {
                    _pingerTimeout = value;
                    OnPropertyChanged(nameof(PingerTimeout));
                }
            }
        }
        private string _pingerData = string.Empty;
        public string PingerData
        {
            get
            {
                return _pingerData;
            }
            set
            {
                if (_pingerData != value)
                {
                    _pingerData = value;
                    OnPropertyChanged(nameof(PingerData));
                }
            }
        }
    }
}
