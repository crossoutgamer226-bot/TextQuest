using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TextQuestGame.Model;
using TextQuestGame.Model.Services;

namespace TextQuestGame.Presenter
{
    public class GamePresenter
    {
        private readonly IGameFacade _game;
        private readonly IGameView _view;

        public GamePresenter(MainForm view, IGameService game)
        {
            _view = view;
            _game = game;

            _view.ChoiceSelected += OnChoiceSelected;
            _view.SaveRequested += OnSaveRequested;
            _view.LoadRequested += OnLoadRequested;
            _view.NewGameRequested += OnNewGameRequested;

            UpdateView();
        }

        private void OnChoiceSelected(int index)
        {
            _game.MakeChoice(index);
            UpdateView();
        }

        private void UpdateView()
        {
            var scene = _game.GetCurrentScene();
            _view.DisplayScene(scene.Text,
                scene.Choices.Select(c => c.Text).ToList());
        }
    }

}
