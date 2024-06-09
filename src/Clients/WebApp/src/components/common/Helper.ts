export const formatErrorMessages = (errors: string[]): string => {
  let errorMessageList = "";

  errors.forEach((error: string) => {
    errorMessageList += error + "\n";
  });

  return errorMessageList;
};
