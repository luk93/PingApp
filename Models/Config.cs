using PingApp.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Models
{
    public class Config : ObservableBaseModel
    {
        private uint _sheetIndex;
        public uint SheetIndex
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
        private uint _startRow;
        public uint StartRow
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
        private uint _startColumn;
        public uint StartColumn
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
        private uint _pingerTimeout;
        public uint PingerTimeout
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
        [MaxLength(10)]
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
        private uint _pingerRepeatCount;
        public uint PingerRepeatCount
        {
            get
            {
                return _pingerRepeatCount;
            }
            set
            {
                if (_pingerRepeatCount != value)
                {
                    _pingerRepeatCount = value;
                    OnPropertyChanged(nameof(PingerRepeatCount));
                }
            }
        }
        private bool _isExcelTemplateShown;
        public bool IsExcelTemplateShown
        {
            get => _isExcelTemplateShown;
            set
            {
                _isExcelTemplateShown = value;
                OnPropertyChanged(nameof(IsExcelTemplateShown));
            }
        }
    }
}
