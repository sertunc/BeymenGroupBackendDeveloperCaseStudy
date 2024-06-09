import * as React from "react";
import { useSnackbar } from "../../contexts/SnackbarContext";
import Avatar from "@mui/material/Avatar";
import Button from "@mui/material/Button";
import CssBaseline from "@mui/material/CssBaseline";
import TextField from "@mui/material/TextField";
import Box from "@mui/material/Box";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import Typography from "@mui/material/Typography";
import Container from "@mui/material/Container";

interface IProps {
  handleLogin: (status: boolean) => void;
}

export default function Login(props: IProps) {
  const { openSnackbar } = useSnackbar();

  const [values, setValues] = React.useState({
    username: "",
    password: "",
  });

  const handleSubmit = (event: any) => {
    event.preventDefault();

    if (values.username === "" && values.password === "") {
      openSnackbar("Lütfen kullanıcı adı ve şifreyi giriniz.", "error");
      return;
    }

    if (values.username === "1" && values.password === "1") {
      props.handleLogin(true);
    } else {
      openSnackbar("Hatalı kullanıcı adı veya şifre.", "error");
    }
  };

  return (
    <Container component="main" maxWidth="xs">
      <CssBaseline />
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <Avatar sx={{ m: 1, bgcolor: "secondary.main" }}>
          <LockOutlinedIcon />
        </Avatar>
        <Typography component="h1" variant="h5">
          Login
        </Typography>
        <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 1 }}>
          <TextField
            size="small"
            margin="normal"
            required
            fullWidth
            label="Username"
            name="username"
            autoFocus
            onChange={(event) =>
              setValues({ ...values, username: event.target.value })
            }
          />
          <TextField
            size="small"
            margin="normal"
            required
            fullWidth
            name="password"
            label="Password"
            type="password"
            onChange={(event) =>
              setValues({ ...values, password: event.target.value })
            }
          />
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            Login
          </Button>
        </Box>
      </Box>
    </Container>
  );
}
