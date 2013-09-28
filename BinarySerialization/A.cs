namespace A
{
    [System.Serializable]
    public class A
    {
        private B.B b;

        public A(string s)
        {
            b = new B.B(s);
        }

        public override string ToString()
        {
            return b.S;
        }
    }
}
