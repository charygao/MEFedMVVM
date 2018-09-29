using System.ComponentModel;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MEFedMVVM.Common;

namespace MEFedMVVMTests.Common
{
    [TestClass]
    public class PropertyObserverTests
    {
        [TestMethod]
        public void Test_DoOnce()
        {
            //ARRANGE
            var notifyPropertyChangedMock = new NotifyPropertyChangedMock();

            const string calledOnce = "1";
            var called = new StringBuilder();

            //ACT
            notifyPropertyChangedMock.OnChanged(x => x.TestProperty).DoOnce(x => called.Append(calledOnce) );
            notifyPropertyChangedMock.TestProperty = "asdasd";
            notifyPropertyChangedMock.TestProperty = "asdasd";

            //ASSERT
            Assert.AreEqual(calledOnce, called.ToString(), "Property changed was not called with DoOnce");
        }

        [TestMethod]
        public void Test_Do()
        {
            //ARRANGE
            var notifyPropertyChangedMock = new NotifyPropertyChangedMock();

            const string calledOnce = "1";
            var called = new StringBuilder();

            //ACT
            notifyPropertyChangedMock.OnChanged(x => x.TestProperty).Do(x => called.Append(calledOnce));
            notifyPropertyChangedMock.TestProperty = "asdasd";
            notifyPropertyChangedMock.TestProperty = "asdase";

            //ASSERT
            Assert.AreEqual(calledOnce + calledOnce, called.ToString(), "Property changed was not called with DoOnce");
        }

        [TestMethod]
        public void Test_Do_With_Unsubscribe()
        {
            //ARRANGE
            var notifyPropertyChangedMock = new NotifyPropertyChangedMock();

            const string calledOnce = "1";
            var called = new StringBuilder();

            //ACT
            using(notifyPropertyChangedMock.OnChanged(x => x.TestProperty).Do(x => called.Append(calledOnce)))
                notifyPropertyChangedMock.TestProperty = "asdasd";

            notifyPropertyChangedMock.TestProperty = "asdasd";

            //ASSERT
            Assert.AreEqual(calledOnce, called.ToString(), "Property changed was not called with DoOnce");
        }
    }

    internal class NotifyPropertyChangedMock : INotifyPropertyChanged
    {
        private string _testProperty;
        public string TestProperty
        {
            get
            {
                return _testProperty;
            }
            set
            {
                _testProperty = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TestProperty"));
            }
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
