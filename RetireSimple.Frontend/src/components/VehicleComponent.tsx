import { style  } from '@mui/system';
import {Box, Button, Divider, Tab, Tabs, Typography, Icon} from '@mui/material';

import {InvestmentVehicle} from '../Interfaces';
import { VehicleModelGraph } from './GraphComponents';
import { deleteVehicle } from '../api/VehicleApi';

export const VehicleComponent = (vehicle: InvestmentVehicle, callback: Function) => {
//deleteInvestment

	return <body style={{backgroundColor: '#DCDCDC', margin: '15px'}}>
		<div style={{width: '900px', paddingLeft: '10px', paddingBottom: '10px', paddingRight: '0px'}}>
			<span>
				<h2> {vehicle.investmentVehicleName} 
					<Button onClick={() => callback()}>
						<Icon style={{color: 'black'}} baseClassName='material-icons'>edit_circle</Icon>
						<Typography variant='body1' component='div' sx={{marginLeft: '10px'}}>
							{/* Delete Vehicle */}
						</Typography>
					</Button>
					<Button onClick={() => deleteVehicle(vehicle.investmentVehicleId)}>
						<Icon style={{color: 'black'}} baseClassName='material-icons'>delete_circle</Icon>
						<Typography variant='body1' component='div' sx={{marginLeft: '10px'}}>
							{/* Delete Vehicle */}
						</Typography>
					</Button>
				</h2>				
			</span>
			<span style={{display: 'flex'}}>
				<div style={{flex: '50%', width: '50px'}}>
					<VehicleModelGraph vehicleId={vehicle.investmentVehicleId} />
				</div>
			</span>
		</div>
	</body>
}