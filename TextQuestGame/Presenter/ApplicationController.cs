using System;
using System.Collections.Generic;
using System.Text;

namespace TextQuestGame.Presenter
{
    internal class ApplicationController
    {
        private GamePresenter _presenter;
        private IGameService _gameService;
        private IGameView _view;
    }
}
