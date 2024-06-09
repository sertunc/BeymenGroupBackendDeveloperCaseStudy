import { FormikErrors } from "formik";
import ConfigModel from "./ConfigModel";

export const configModelValidator = (
  values: ConfigModel
): FormikErrors<ConfigModel> => {
  const errors: FormikErrors<ConfigModel> = {};

  if (!values.name) {
    errors.name = "Name alanı boş bırakılamaz!";
  }

  if (values.type === "") {
    errors.type = "Type alanı boş bırakılamaz!";
  }

  if (!values.value) {
    errors.value = "Value alanı boş bırakılamaz!";
  }

  if (values.applicationName === "") {
    errors.applicationName = "Application Name alanı boş bırakılamaz!";
  }

  if (!compareValueAndType({ value: values.value, type: values.type })) {
    errors.value = "Value alanı hatalı girilmiştir!";
  }

  return errors;
};

interface ValueWithType {
  value: string;
  type: string;
}

const compareValueAndType = (input: ValueWithType): boolean => {
  let result = false;

  switch (input.type) {
    case "System.String":
      result = typeof input.value === "string";
      break;
    case "System.Int32":
      result = /^[-]?\d+$/.test(input.value);
      break;
    case "System.Boolean":
      result = input.value === "true" || input.value === "false";
      break;
    case "System.Double":
      result = /^[-]?\d+,\d+$/.test(input.value);
      //result = true;
      break;
    default:
      result = false;
  }

  return result;
};
