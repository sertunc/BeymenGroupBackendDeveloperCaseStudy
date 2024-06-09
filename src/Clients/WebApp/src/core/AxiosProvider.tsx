import React, { useState } from "react";
import axios from "axios";
import { v4 as uuidv4 } from "uuid";
import { CircularProgress, Dialog, DialogContent } from "@mui/material";

export default function AxiosProvider(props: any) {
  const [showInprogress, setInprogressCount] = useState(false);

  const requestIdKey = "X-Request-Id";
  axios.defaults.baseURL = import.meta.env.VITE_GATEWAY_BASEURL;

  axios.interceptors.request.use(
    (config) => {
      config.headers[requestIdKey] = uuidv4();
      return config;
    },
    (error) => {
      setInprogressCount(false);
      return Promise.reject(error);
    }
  );

  axios.interceptors.response.use(
    (response: any) => {
      setInprogressCount(false);
      return response;
    },
    (error) => {
      setInprogressCount(false);
      return Promise.reject(error);
    }
  );

  return (
    <React.Fragment>
      {React.Children.only(props.children)}
      <Dialog open={showInprogress}>
        <DialogContent>
          <CircularProgress />
        </DialogContent>
      </Dialog>
    </React.Fragment>
  );
}
