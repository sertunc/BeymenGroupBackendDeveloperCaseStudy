import { MenuItem } from "@mui/material";
import FormikTextField from "./FormikTextField";

const FormikSelectField = ({ values, ...props }: any) => {
  return (
    <FormikTextField select type="text" {...props}>
      {values.map((item: any) => (
        <MenuItem key={item} value={item}>
          {item}
        </MenuItem>
      ))}
    </FormikTextField>
  );
};

export default FormikSelectField;
