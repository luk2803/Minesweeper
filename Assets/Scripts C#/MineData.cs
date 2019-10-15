using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

namespace Assets.Scripts_C_
{
    public class MineData : IMineData
    {
        public MineData(Mine mineInstance, List<Sprite> openMines, List<Sprite> advancedMines)
        {
            this.spriteRenderer = mineInstance.GetComponent<SpriteRenderer>();
            this.openMines = openMines;
            this.advancedMines = advancedMines;
        }

        private List<Sprite> openMines;
        private List<Sprite> advancedMines;
        private SpriteRenderer spriteRenderer;
        private int _position;
        public int Position
        {
            get => _position; 
            set => _position = value; 
        }

        private int _minesInNear = 0;
        public int MinesInNear
        {
            get => _minesInNear;
            set => _minesInNear = value;
        }

        private bool _isMine = false;
        public bool IsMine
        {
            get => _isMine; 
            set => _isMine = value; 
        }

        private bool _isNotAllowedToBeBomb = false;
        public bool IsNotAllowedToBeBomb
        {
            get => _isNotAllowedToBeBomb; 
            set => _isNotAllowedToBeBomb = value; 
        }

        private MineState _state;

        public MineState State
        {
            get => _state; 
            set
            {
                switch (value)
                {
                    case MineState.open:
                        {
                            _state = MineState.open;
                            spriteRenderer.sprite = openMines[0];
                            break;
                        }
                    case MineState.one:
                        {
                            _state = MineState.one;
                            spriteRenderer.sprite = openMines[1];
                            break;
                        }
                    case MineState.two:
                        {
                            _state = MineState.two;
                            spriteRenderer.sprite = openMines[2];
                            break;
                        }
                    case MineState.three:
                        {
                            _state = MineState.three;
                            spriteRenderer.sprite = openMines[3];
                            break;
                        }
                    case MineState.four:
                        {
                            _state = MineState.four;
                            spriteRenderer.sprite = openMines[4];
                            break;
                        }
                    case MineState.five:
                        {
                            _state = MineState.five;
                            spriteRenderer.sprite = openMines[5];
                            break;
                        }
                    case MineState.six:
                        {
                            _state = MineState.six;
                            spriteRenderer.sprite = openMines[6];
                            break;
                        }
                    case MineState.seven:
                        {
                            _state = MineState.seven;
                            spriteRenderer.sprite = openMines[7];
                            break;
                        }
                    case MineState.eight:
                        {
                            _state = MineState.eight;
                            spriteRenderer.sprite = openMines[8];
                            break;
                        }
                    case MineState.clicked:
                        {
                            _state = MineState.clicked;
                            spriteRenderer.sprite = advancedMines[0];
                            break;
                        }
                    case MineState.mine:
                        {
                            _state = MineState.mine;
                            spriteRenderer.sprite = advancedMines[1];
                            break;
                        }
                    case MineState.bomb:
                        {
                            _state = MineState.bomb;
                            spriteRenderer.sprite = advancedMines[2];
                            break;
                        }
                    case MineState.redbomb:
                        {
                            _state = MineState.redbomb;
                            spriteRenderer.sprite = advancedMines[3];
                            break;
                        }
                    case MineState.mineFlag:
                    {
                        _state = MineState.mineFlag;
                        spriteRenderer.sprite = advancedMines[4];
                        break;
                    }
                    case MineState.flagclicked:
                    {
                        _state = MineState.flagclicked;
                        break;
                    }
                }


            }
        }

        public override string ToString()
        {
            return $"Position: {Position}, MinesInNear: {MinesInNear}, IsMine: {IsMine}, IsNotAllowedToBeBomg: {IsNotAllowedToBeBomb}, MineState: {State.ToString()}";

        }
    }
}
