import React from "react";
import { Chip } from "@mui/material";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import CancelIcon from "@mui/icons-material/Cancel";

interface IProps {
  isActive: boolean;
}

const StatusChip: React.FC<IProps> = ({ isActive }) => (
  <Chip
    color="primary"
    icon={isActive ? <CheckCircleIcon /> : <CancelIcon />}
    label={isActive ? "Aktif" : "Pasif"}
  />
);

export default StatusChip;
