import { InputAdornment, TextField } from "@mui/material";

const FormikTextField = ({ fieldName, formik, ...props }: any) => (
  <TextField
    fullWidth
    size="small"
    variant="outlined"
    InputProps={{
      startAdornment: <InputAdornment position="start"></InputAdornment>,
    }}
    name={fieldName}
    value={formik.values[fieldName]}
    onChange={formik.handleChange}
    onBlur={formik.handleBlur}
    error={formik.touched[fieldName] && Boolean(formik.errors[fieldName])}
    helperText={formik.touched[fieldName] && formik.errors[fieldName]}
    {...props}
  />
);

export default FormikTextField;
