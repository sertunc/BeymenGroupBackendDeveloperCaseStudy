import { useEffect, useState } from "react";
import axios from "axios";
import { useFormik } from "formik";
import { useSnackbar } from "../../contexts/SnackbarContext";
import FormikTextField from "../common/FormikTextField";
import FormikSelectField from "../common/FormikSelectField";
import {
  Box,
  Button,
  Card,
  CardActions,
  CardContent,
  CardHeader,
  Checkbox,
  FormControlLabel,
  Grid,
  Modal,
} from "@mui/material";
import ConfigModel from "./models/ConfigModel";
import { configModelValidator } from "./models/ConfigModelValidator";
import { formatErrorMessages } from "../common/Helper";

const cardStyle = {
  position: "absolute" as "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 500,
  boxShadow: 24,
};

interface IProps {
  id: string;
  open: boolean;
  onClose: () => void;
}
export default function ConfigurationEdit(props: IProps) {
  const { openSnackbar } = useSnackbar();

  const [valueTypes, setValueTypes] = useState<string[]>([]);
  const [applicationNames, setApplicationNames] = useState<string[]>([]);

  useEffect(() => {
    (async () => {
      if (props.id.length > 0) {
        try {
          const [
            getValueTypesResponse,
            getApplicationNamesResponse,
            configurationResponse,
          ] = await Promise.all([
            axios.get("configuration/getvaluetypes"),
            axios.get("configuration/getapplicationnames"),
            axios.get(`configuration/${props.id}`),
          ]);

          setValueTypes(getValueTypesResponse.data.data);
          setApplicationNames(getApplicationNamesResponse.data);
          formik.setValues(configurationResponse.data.data);
        } catch (error) {
          openSnackbar(error, "error");
        }
      }
    })();
  }, [props.id]);

  const formik = useFormik({
    initialValues: new ConfigModel(),
    validate: configModelValidator,
    onSubmit: (values, actions) => {
      axios.post("configuration", values).then((response) => {
        if (response.data.isSuccessful === true) {
          actions.resetForm({ values: new ConfigModel() });
          props.onClose();
          openSnackbar(response.data.message, "success");
        } else {
          openSnackbar(formatErrorMessages(response.data.errors), "error");
        }
      });
    },
  });

  return (
    <Modal open={props.open} onClose={props.onClose}>
      <Box component="form" onSubmit={formik.handleSubmit}>
        <Card sx={cardStyle}>
          <CardHeader title="Konfigürasyon Düzenle" />
          <CardContent>
            <Grid container spacing={2}>
              <Grid item xs={12}>
                <FormikTextField
                  label="Name"
                  type="text"
                  fieldName="name"
                  formik={formik}
                />
              </Grid>
              <Grid item xs={12}>
                <FormikSelectField
                  label="Type"
                  formik={formik}
                  value={formik.values.type}
                  values={valueTypes}
                  onChange={(event: any) => {
                    formik.setFieldValue("type", event.target.value);
                  }}
                />
              </Grid>
              <Grid item xs={12}>
                <FormikTextField
                  label="Value"
                  type="text"
                  fieldName="value"
                  formik={formik}
                />
              </Grid>
              <Grid item xs={12}>
                <FormikSelectField
                  label="Application Name"
                  formik={formik}
                  value={formik.values.applicationName}
                  values={applicationNames}
                  onChange={(event: any) => {
                    formik.setFieldValue("applicationName", event.target.value);
                  }}
                />
              </Grid>
              <Grid item xs={12}>
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={formik.values.isActive}
                      onChange={() => {
                        formik.setFieldValue(
                          "isActive",
                          !formik.values.isActive
                        );
                      }}
                    />
                  }
                  label="Aktif"
                />
              </Grid>
            </Grid>
          </CardContent>
          <CardActions>
            <Button
              type="submit"
              color="primary"
              variant="contained"
              size="small"
            >
              DÜZENLE
            </Button>
          </CardActions>
        </Card>
      </Box>
    </Modal>
  );
}
