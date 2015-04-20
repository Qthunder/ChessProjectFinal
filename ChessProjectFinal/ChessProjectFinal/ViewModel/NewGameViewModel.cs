using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ChessProjectFinal.Common;
using ChessProjectFinal.Entities;

namespace ChessProjectFinal.ViewModel
{
    public class NewGameViewModel :BasePropertyChanged
    {
        #region PRIVATE BACKING FIELDS
        private PlayerType whitePlayerType;
        private PlayerType blackPlayerType;
        private int whiteDepth;
        private int blackDepth;
        private int whiteTime;
        private int blackTime;
        private bool whiteUsingPV;
        private bool blackUsingPV;

        #endregion

        private void cancel(object obj)
        {
            IsCanceled = true;
            CloseAction();
        }

        private void okAction(object obj)
        {
            CloseAction();
        }
        public Action CloseAction { get; set; }
        public ICommand OkCommand
        {
            get
            {
               return new RelayCommand(okAction);
            }
        }
        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand(cancel);
            }
        }
        public ObservableCollection<PlayerType> PlayerTypes
        {
            get
            {
                return new ObservableCollection<PlayerType>(){PlayerType.HUMAN,PlayerType.AI};
            }
        }

        public ObservableCollection<int> Depths
        {
            get
            {
                return new ObservableCollection<int> { 1,2,3,4,5,6,7,8,9,10 };
            }
        }

        public bool IsCanceled { get; set; }
        #region WHITE PLAYER SETTINGS
        public PlayerType WhitePlayerType
        {
            get
            {
                return whitePlayerType;
            }
            set
            {
                whitePlayerType = value;
                RaisePropertyChanged(()=>WhitePlayerType);
            }
        }
        public int WhiteDepth
        {
            get
            {
                return whiteDepth;
            }
            set
            {
                whiteDepth = value;
                RaisePropertyChanged(() => WhiteDepth);
            }
        }
        public int WhiteTime
        {
            get
            {
                return whiteTime;
            }
            set
            {
                whiteTime = value;
                RaisePropertyChanged(() => WhiteTime);
            }
        }

        public bool WhiteUsingPV
        {
            get { return whiteUsingPV; }
            set
            {
                whiteUsingPV = value;
                RaisePropertyChanged(()=>WhiteUsingPV);
            }
        }
        #endregion
        #region BLACK PLAYER SETTINGS
        public PlayerType BlackPlayerType
        {
            get
            {
                return blackPlayerType;
            }
            set
            {
                blackPlayerType = value;
                RaisePropertyChanged(()=>BlackPlayerType);
            }

        }
        public int BlackDepth
        {
            get
            {
                return blackDepth;
            }
            set
            {
                blackDepth = value;
                RaisePropertyChanged(() => BlackDepth);
            }
        }
        public int BlackTime
        {
            get
            {
                return blackTime;
            }
            set
            {
                blackTime = value;
                RaisePropertyChanged(() => BlackTime);
            }
        }
        public bool BlackUsingPV
        {
            get { return blackUsingPV; }
            set
            {
                blackUsingPV = value;
                RaisePropertyChanged(() => blackUsingPV);
            }
        }
        #endregion



        public NewGameViewModel()
        {
            WhitePlayerType=PlayerType.HUMAN;
            WhiteDepth = 4;
            WhiteTime = 30;
            WhiteUsingPV = true;
            BlackPlayerType=PlayerType.HUMAN;
            BlackDepth = 4;
            BlackTime = 30;
            IsCanceled = false;
            var view = new View.NewGameView { DataContext = this };
            CloseAction = view.Close;
            view.ShowDialog();
            
        }

    }
}
