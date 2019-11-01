using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.Analytics;

namespace xLayout.Example
{

    public class FollowValue<T> : System.IObservable<T>
    {
        private readonly IObservable<T> _truth;
        private readonly Func<T, T, float, T> _follower;
        private readonly T start;
        private readonly T end;


        public FollowValue(IObservable<T> truth, System.Func<T, T, float, T> follower)
        {
            _truth = truth;
            _follower = follower;
        }
        
        public FollowValue(T start, T end, System.Func<T, T, float, T> follower)
        {
            this.start = start;
            this.end = end;
            _follower = follower;
        }
        
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (_truth == null)
                return new CoreNonObservable(this, observer);
            return new Core(this, observer);
        }

        private class CoreNonObservable : System.IDisposable
        {
            private Tick t = new Tick();

            private T currentValue;
            private T destValue;

            private System.IDisposable tickSub;
            
            public CoreNonObservable(FollowValue<T> followValue, IObserver<T> observer)
            {
                destValue = followValue.end;
                currentValue = followValue.start;

                tickSub = t.Subscribe(delta =>
                {
                    currentValue = followValue._follower(currentValue, destValue, delta);
                    observer.OnNext(currentValue);
                    
                    if (!currentValue.Equals(destValue)) 
                        return;
                    
                    observer.OnCompleted();
                    Dispose();
                });
            }

            public void Dispose()
            {
                tickSub?.Dispose();
                tickSub = null;
            }   
        }
        
        private class Core : System.IDisposable
        {
            private readonly IObserver<T> _observer;
            private Tick t = new Tick();

            private T currentValue;
            private T destValue;

            private System.IDisposable truthSub;
            private System.IDisposable tickSub;
            
            public Core(FollowValue<T> followValue, IObserver<T> observer)
            {
                _observer = observer;
                truthSub = followValue._truth.Subscribe(truth => { destValue = truth; }, OnError, Complete);
                currentValue = destValue;

                if (truthSub != null)
                {
                    tickSub = t.Subscribe(delta =>
                    {
                        currentValue = followValue._follower(currentValue, destValue, delta);
                        observer.OnNext(currentValue);
                    });       
                }
            }

            private void OnError(Exception exception)
            {
                _observer.OnError(exception);
                Dispose();
            }

            private void Complete()
            {
                _observer.OnCompleted();
                Dispose();
            }

            public void Dispose()
            {
                truthSub?.Dispose();
                tickSub?.Dispose();
                truthSub = null;
                tickSub = null;
            }
        }
    }
    
    public class Tick : System.IObservable<float>
    {
        public IDisposable Subscribe(IObserver<float> observer)
        {
            return new Core(observer);
        }

        private class Core : System.IDisposable
        {
            private bool disposed;
            private readonly IObserver<float> _observer;

            public Core(IObserver<float> observer)
            {
                _observer = observer;
                MainThreadDispatcher.StartUpdateMicroCoroutine(Worker());
            }

            private IEnumerator Worker()
            {
                while (!disposed)
                {
                    _observer.OnNext(Time.deltaTime);
                    yield return null;                    
                }
            }

            public void Dispose()
            {
                disposed = true;
                _observer.OnCompleted();
            }
        }
    }
}