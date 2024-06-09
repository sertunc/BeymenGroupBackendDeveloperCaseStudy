import React, { createContext, useContext, useState } from "react";
import { OverridableStringUnion } from "@mui/types";
import Snackbar from "@mui/material/Snackbar";
import Alert, {
  AlertColor,
  AlertPropsColorOverrides,
} from "@mui/material/Alert";

interface SnackbarStatusType {
  isOpen: boolean;
  message: string;
  alertColor?: OverridableStringUnion<AlertColor, AlertPropsColorOverrides>;
}

const SnackbarContext = createContext<any>(null);

export const SnackbarProvider = ({
  children,
}: {
  children: React.ReactNode;
}) => {
  const [snackbarStatus, setSnackbarStatus] = useState<SnackbarStatusType>({
    isOpen: false,
    message: "",
  });

  const openSnackbar = (
    message: string,
    alertColor: OverridableStringUnion<AlertColor, AlertPropsColorOverrides>
  ) => {
    setSnackbarStatus({
      ...snackbarStatus,
      isOpen: true,
      message,
      alertColor,
    });
  };

  const closeSnackbar = () => {
    setSnackbarStatus({
      ...snackbarStatus,
      isOpen: false,
      message: "",
    });
  };

  const value = {
    openSnackbar,
    closeSnackbar,
  };

  return (
    <SnackbarContext.Provider value={value}>
      {children}
      <Snackbar
        open={snackbarStatus.isOpen}
        anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
        autoHideDuration={5000}
        onClose={closeSnackbar}
      >
        <Alert
          variant="filled"
          onClose={closeSnackbar}
          severity={snackbarStatus.alertColor}
          sx={{ width: "100%" }}
        >
          {snackbarStatus.message}
        </Alert>
      </Snackbar>
    </SnackbarContext.Provider>
  );
};

export const useSnackbar = () => {
  return useContext(SnackbarContext);
};
