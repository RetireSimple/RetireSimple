import { style  } from '@mui/system';
import {Box, Button, Divider, Tab, Tabs, Typography, Icon} from '@mui/material';
import React, { useState } from 'react';

import {InvestmentVehicle} from '../Interfaces';
import { VehicleModelGraph } from './GraphComponents';
import { deleteVehicle } from '../api/VehicleApi';


export const EditableTable = () => {
	//deleteInvestment
	const [v1, setV1] = useState(0);

    
	return <body style={{backgroundColor: '#DCDCDC', margin: '15px'}}>
		<span>  {`${v1} to ${v1}`} </span>
	</body>
}
export default EditableTable;
