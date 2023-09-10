using Microsoft.Xna.Framework;
using System;

namespace HorrorShorts_Game.Algorithms.Tweener
{
    public enum TweenState : byte
    {
        Doing,
        Paused,
        Finished
    }
    public enum TweenBucleType : byte
    {
        None,
        Forward,
        Reverse,
        PingPong
    }

    public class Tween<T>
    {
        private readonly Func<float, float> _function; //Lineal to function value
        private readonly Func<float, T, T, T> _converterFunction; //Function value to final value

        public TweenState State { get => _state; }
        private TweenState _state = TweenState.Finished;

        private float _linearValue = 0f;
        private float _functionValue = 0f;
        private T _realValue = default;
        public T Value { get => _realValue; }
        public float FunctionValue { get => _functionValue; }
        public float LinearValue { get => _linearValue; }


        private float _elapsedTime = 0f;
        private readonly float _duration = 1000f;

        private readonly float _delay = 0f;
        private float _delayElapsed = 0f;
        private bool _doingDelay = false;

        private readonly T _startValue;
        private readonly T _endValue;


        public bool Bucle
        {
            get => _bucleType != TweenBucleType.None;
            set
            {
                if (value)
                {
                    _bucleType = TweenBucleType.Forward;
                    _updateAction = UpdateForwardBucle;
                }
                else
                {
                    _bucleType = TweenBucleType.None;
                    _updateAction = UpdateNoBucle;
                }
            }
        }
        public TweenBucleType BucleType
        {
            get => _bucleType;
            set
            {
                _bucleType = value;
                _updateAction = _bucleType switch
                {
                    TweenBucleType.None => UpdateNoBucle,
                    TweenBucleType.Forward => UpdateForwardBucle,
                    TweenBucleType.Reverse => UpdateReverseBucle,
                    TweenBucleType.PingPong => UpdatePingPongBucle,
                    _ => throw new NotImplementedException("Not implemented bucle type for " + _bucleType)
                };
            }
        }
        private TweenBucleType _bucleType = TweenBucleType.None;
        private bool _pingPongDirection = false;

        public float SpeedMod { get => _speedMod; set => _speedMod = value; }
        private float _speedMod = 1f;
        public float LinealMod { get => _linealMod; set => _linealMod = value; }
        private float _linealMod = 0f;

        private Action _updateAction;
        private bool _isUpdated = false;
        public bool IsUpdated { get => _isUpdated; }

        public Tween(T start, T end, float duration, float delay = 0f, Func<float, float> function = null, Func<float, T, T, T> converter = null)
        {
            _startValue = start;
            _endValue = end;
            _duration = duration;
            _delay = delay;

            _updateAction = UpdateNoBucle;

            //Math Function
            if (function == null)
                _function = TweenFunctions.Linear;
            else _function = function;

            //Converter
            Type valueType = typeof(T);
            if (converter == null)
            {
                if (valueType == typeof(float))
                    _converterFunction = TweenConverters.FloatConverter;
                else if (valueType == typeof(int))
                    _converterFunction = TweenConverters.IntConverter;
                else if (valueType == typeof(short))
                    _converterFunction = TweenConverters.ShortConverter;
                else if (valueType == typeof(byte))
                    _converterFunction = TweenConverters.ByteConverter;
                else if (valueType == typeof(Point))
                    _converterFunction = TweenConverters.PointConverter;
                else if (valueType == typeof(Vector2))
                    _converterFunction = TweenConverters.Vector2Converter;
                else if (valueType == typeof(Color))
                    _converterFunction = TweenConverters.ColorConverter;
                else throw new NotSupportedException($"Not supported type for tween: {typeof(T)}");
            }
            else _converterFunction = converter;
        }

        public void Update()
        {
            _isUpdated = false;
            if (_state != TweenState.Doing) return;

            if (_doingDelay)
            {
                _delayElapsed += (float)Core.GameTime.ElapsedGameTime.TotalMilliseconds;
                if (_delayElapsed >= _delay) _doingDelay = false;
                else return;
            }

            _updateAction.Invoke();
            UpdateFunctionValue(_function.Invoke(_linearValue));
            _realValue = _converterFunction.Invoke(_functionValue, _startValue, _endValue);

            _elapsedTime += (float)Core.GameTime.ElapsedGameTime.TotalMilliseconds * _speedMod;
            _isUpdated = true;
        }


        private void UpdateNoBucle()
        {
            if (_elapsedTime >= _duration)
            {
                UpdateLinealValue(1f);
                UpdateFunctionValue(_linearValue);
                _realValue = _endValue;
                _state = TweenState.Finished;
            }
            else UpdateLinealValue(_elapsedTime / _duration);
        }
        private void UpdateForwardBucle()
        {
            if (_elapsedTime >= _duration)
                _elapsedTime -= _duration;

            UpdateLinealValue(_elapsedTime / _duration);
        }
        private void UpdateReverseBucle()
        {
            if (_elapsedTime >= _duration)
                _elapsedTime -= _duration;

            UpdateLinealValue(1f - (_elapsedTime / _duration));
        }
        private void UpdatePingPongBucle()
        {
            if (_elapsedTime >= _duration)
            {
                _elapsedTime -= _duration;
                _pingPongDirection = !_pingPongDirection;
            }

            if (_pingPongDirection)
                UpdateLinealValue(1f - (_elapsedTime / _duration));
            else UpdateLinealValue(_elapsedTime / _duration);
        }

        private void UpdateLinealValue(float value)
        {
            value += _linealMod;
            if (value > 1f) value -= 1f;
            _linearValue = value;
        }
        private void UpdateFunctionValue(float value)
        {
            value -= _linealMod;
            if (value < 0f) value += 1f;
            _functionValue = value;
        }

        public void Refresh(ref T value)
        {
            //if (_state != TweenState.Doing) return;
            value = _realValue;
        }

        public void Start()
        {
            _state = TweenState.Doing;

            _elapsedTime = 0f;
            _updateAction.Invoke();
            UpdateFunctionValue(_function.Invoke(_linearValue));
            _realValue = _converterFunction.Invoke(_functionValue, _startValue, _endValue);
            _delayElapsed = 0f;
            _doingDelay = _delay > 0;

            _isUpdated = true;
        }
        public void Pause()
        {
            if (_state != TweenState.Doing) return;
            _state = TweenState.Paused;
        }
        public void Stop()
        {
            if (_state == TweenState.Finished) return;
            _state = TweenState.Finished;
        }
        public void Resume()
        {
            if (_state != TweenState.Paused) return;
            _state = TweenState.Doing;
        }
        public void Complete()
        {
            if (_state == TweenState.Finished) return;
            _state = TweenState.Finished;

            _elapsedTime = _duration;
            _updateAction.Invoke();
            UpdateFunctionValue(_function.Invoke(_linearValue));
            _realValue = _converterFunction.Invoke(_functionValue, _startValue, _endValue);

            _isUpdated = true;
        }
    }
}
