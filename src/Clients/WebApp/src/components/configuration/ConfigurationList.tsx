import {
  Button,
  IconButton,
  InputAdornment,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
  Toolbar,
  Tooltip,
  Typography,
} from "@mui/material";
import ConfigModel from "./models/ConfigModel";
import StatusChip from "../common/StatusChip";
import DeleteIcon from "../common/icons/DeleteIcon";
import EditIcon from "../common/icons/EditIcon";

interface IProps {
  model: ConfigModel[];
  searchText: string;
  handleSearchChange: (event: any) => void;
  handleAdd: (event: any) => void;
  handleEdit: (id: string) => void;
  handleDelete: (model: ConfigModel) => void;
}

export default function ConfigurationList(props: IProps) {
  return (
    <>
      <Toolbar>
        <Typography variant="h6" style={{ margin: "-23px" }}>
          Konfigürasyon Listesi
        </Typography>
        <div style={{ marginLeft: "35px" }}>
          <TextField
            size="small"
            variant="outlined"
            label="Name ara"
            value={props.searchText}
            onChange={props.handleSearchChange}
            InputProps={{
              startAdornment: <InputAdornment position="start" />,
            }}
          />
        </div>
        <div style={{ flexGrow: 1, textAlign: "right" }}>
          <Button
            color="primary"
            variant="contained"
            size="small"
            style={{ marginRight: "-23px" }}
            onClick={props.handleAdd}
          >
            EKLE
          </Button>
        </div>
      </Toolbar>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Name</TableCell>
              <TableCell>Type</TableCell>
              <TableCell>Value</TableCell>
              <TableCell>IsActive</TableCell>
              <TableCell>ApplicationName</TableCell>
              <TableCell></TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {props.model.map((item) => (
              <TableRow key={item.id}>
                <TableCell component="th" scope="row">
                  {item.id}
                </TableCell>
                <TableCell>{item.name}</TableCell>
                <TableCell>{item.type}</TableCell>
                <TableCell>{item.value.toString()}</TableCell>
                <TableCell>
                  <StatusChip isActive={item.isActive} />
                </TableCell>
                <TableCell>{item.applicationName}</TableCell>
                <TableCell align="center">
                  <Tooltip title="Güncelle">
                    <IconButton onClick={() => props.handleEdit(item.id)}>
                      <EditIcon style={{ color: "#2BB673" }} />
                      {/* Stiller tek bir yerde toplanır */}
                    </IconButton>
                  </Tooltip>
                  <Tooltip title="Sil">
                    <IconButton onClick={() => props.handleDelete(item)}>
                      <DeleteIcon style={{ color: "#D9433C" }} />
                    </IconButton>
                  </Tooltip>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </>
  );
}
