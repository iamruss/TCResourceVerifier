using Ninject;

namespace TCResourceVerifier
{
    public class Verifier
    {
        private IKernel _kernel;

        public Verifier()
        {
            _kernel = new StandardKernel(new SimpleModule());
        }

        public void Process(string rootFolder)
        {
            
        }
    }
}