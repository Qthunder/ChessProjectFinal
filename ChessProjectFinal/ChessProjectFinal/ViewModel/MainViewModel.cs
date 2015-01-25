using ChessProjectFinal.Common;

namespace ChessProjectFinal.ViewModel
{
    public class MainViewModel :BasePropertyChanged
    {
        private BoardViewModel boardViewModel;

        public BoardViewModel BoardViewModel
        {
            get { return boardViewModel; }
            set
            { 
                boardViewModel = value;
                RaisePropertyChanged(() =>BoardViewModel);
            }
        }

        public MainViewModel()
        {
            BoardViewModel = new BoardViewModel();
        }
    }
}
