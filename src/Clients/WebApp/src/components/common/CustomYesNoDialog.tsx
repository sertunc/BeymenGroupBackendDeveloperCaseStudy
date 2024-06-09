import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from "@mui/material";

interface IProps {
  titleMessage: string;
  contentMessage: string;
  open: boolean;
  onClose: () => void;
  onYesClick: () => void;
}

export default function CustomYesNoDialog(props: IProps) {
  return props.open === true ? (
    <Dialog open={props.open} onClose={props.onClose}>
      <DialogTitle>{props.titleMessage}</DialogTitle>
      <DialogContent>
        <DialogContentText>{props.contentMessage}</DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button
          size="small"
          onClick={props.onClose}
          variant="contained"
          color="primary"
          autoFocus
        >
          HAYIR
        </Button>
        <Button
          onClick={props.onYesClick}
          size="small"
          variant="contained"
          color="primary"
        >
          EVET
        </Button>
      </DialogActions>
    </Dialog>
  ) : null;
}
