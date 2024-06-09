import { useState } from "react";
import AxiosProvider from "./core/AxiosProvider";
import { SnackbarProvider } from "./contexts/SnackbarContext";
import Layout from "./Layout";
import Login from "./components/auth/Login";
import ConfigurationContainer from "./components/configuration/ConfigurationContainer";

function App(props: any) {
  const [loginStatus, setLoginStatus] = useState(false);

  const handleLogin = (status: boolean) => {
    setLoginStatus(status);
  };

  return (
    <SnackbarProvider>
      <AxiosProvider {...props}>
        <Layout>
          {loginStatus === false ? (
            <Login handleLogin={handleLogin} />
          ) : (
            <ConfigurationContainer />
          )}
        </Layout>
      </AxiosProvider>
    </SnackbarProvider>
  );
}

export default App;
