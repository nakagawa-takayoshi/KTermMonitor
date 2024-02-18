using KtermMonitor.Controller.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KtermMonitor.Controller
{
    internal class FwUpdatgeSubjectObservable: IObserbavle
    {
        private int _state;
        private string _message;
        private List<IObserver> _observers = new List<IObserver>();
        private ISubject _subject;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FwUpdatgeSubjectObservable(ISubject subject)
        {
            _subject = subject;
        }

        /// <summary>
        /// 購読開始
        /// </summary>
        public IDisposable Subscribe(IObserver observer)
        {
            if (observer is null) return Disposable.Empty;

            _observers.Add(observer);

            return Disposable.Create(() => _observers.Remove(observer));
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Execution(_subject);
            }
        }
    }
}
