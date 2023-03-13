import {Box, Typography} from '@mui/material';
import React from 'react';

export interface VehicleListItemProps {
	vehicleName: string;
	vehicleType: string;
}

export const VehicleListItem = (props: VehicleListItemProps) => {
	return <Box>
		<Typography variant='body1' component='div' sx={{flexGrow: 1, textDecoration: 'none'}}>
			{props.vehicleName}
		</Typography>
		<Typography variant='body2' component='div' sx={{flexGrow: 1}}>
			Type: {props.vehicleType}
		</Typography>
	</Box>;
};

