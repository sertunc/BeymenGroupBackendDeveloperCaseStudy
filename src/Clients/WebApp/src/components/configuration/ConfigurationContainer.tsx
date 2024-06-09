import { useEffect, useState } from "react";
import axios from "axios";
import { useSnackbar } from "../../contexts/SnackbarContext";
import { formatErrorMessages } from "../common/Helper";
import { Grid } from "@mui/material";
import ConfigurationList from "./ConfigurationList";
import ConfigModel from "./models/ConfigModel";
import ConfigurationAdd from "./ConfigurationAdd";
import ConfigurationEdit from "./ConfigurationEdit";
import CustomYesNoDialog from "../common/CustomYesNoDialog";

export default function ConfigurationContainer() {
  const { openSnackbar } = useSnackbar();

  const [configurations, setConfigurations] = useState<ConfigModel[]>([]);
  const [filteredConfigurations, setFilteredConfigurations] = useState<
    ConfigModel[]
  >([]);

  const [searchText, setSearchText] = useState<string>("");
  const [openAdd, setAddToogle] = useState(false);
  const [openEdit, setEditToogle] = useState({
    isOpen: false,
    id: "",
  });
  const [openDelete, setDeleteToogle] = useState({
    isOpen: false,
    model: {} as ConfigModel,
  });

  const handleAdd = () => {
    setAddToogle(!openAdd);
  };

  const handleEdit = (id: string) => {
    setEditToogle({
      ...openEdit,
      isOpen: true,
      id,
    });
  };

  const handleDelete = (model: ConfigModel) => {
    setDeleteToogle({
      ...openDelete,
      isOpen: true,
      model,
    });
  };

  const deleteConfiguration = async () => {
    const response = await axios.delete("configuration", {
      data: openDelete.model,
    });

    if (response.data.isSuccessful) {
      openSnackbar(response.data.message, "success");
    } else {
      openSnackbar(formatErrorMessages(response.data.errors), "error");
    }

    setDeleteToogle({
      ...openDelete,
      isOpen: false,
      model: {} as ConfigModel,
    });
  };

  const handleSearchChange = (event: any) => {
    setSearchText(event.target.value);
  };

  useEffect(() => {
    (async () => {
      try {
        //kayıtların fazla olması durumuna göre sayfalama ile çekilir
        const response = await axios.get("configuration");
        setConfigurations(response.data.data);
      } catch (error) {
        openSnackbar(error, "error");
      }
    })();
  }, [openAdd, openEdit.isOpen, openDelete.isOpen]);

  useEffect(() => {
    setFilteredConfigurations(
      configurations.filter((config) =>
        config.name.toLowerCase().includes(searchText.toLowerCase())
      )
    );
  }, [configurations, searchText]);

  return (
    <>
      <Grid container spacing={2}>
        <Grid item xs={12}>
          <ConfigurationList
            model={filteredConfigurations}
            searchText={searchText}
            handleSearchChange={handleSearchChange}
            handleAdd={handleAdd}
            handleEdit={handleEdit}
            handleDelete={handleDelete}
          />
        </Grid>
      </Grid>
      {openAdd && (
        <ConfigurationAdd
          open={openAdd}
          onClose={() => setAddToogle(!openAdd)}
        />
      )}
      {openEdit.isOpen && (
        <ConfigurationEdit
          id={openEdit.id}
          open={openEdit.isOpen}
          onClose={() =>
            setEditToogle({
              ...openEdit,
              isOpen: false,
              id: "",
            })
          }
        />
      )}
      {openDelete.isOpen && (
        <CustomYesNoDialog
          titleMessage="Uyarı"
          contentMessage="Konfigürasyonu silmek istediğinize emin misiniz?"
          open={openDelete.isOpen}
          onYesClick={deleteConfiguration}
          onClose={() =>
            setDeleteToogle({
              ...openDelete,
              isOpen: false,
              model: {} as ConfigModel,
            })
          }
        />
      )}
    </>
  );
}
