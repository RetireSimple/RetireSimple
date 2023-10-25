
import {yupResolver} from '@hookform/resolvers/yup';
import {Box, Button, Divider, Tab, Tabs, Typography, Icon} from '@mui/material';
import React from 'react';
import {FormProvider, useForm, useFormState} from 'react-hook-form';

import {FieldValues} from 'react-hook-form/dist/types';
import {useFormAction, useLoaderData, useSubmit} from 'react-router-dom';
import {Investment, Portfolio, ApiPresetData} from '../Interfaces';
import {getAnalysisPresets} from '../api/ApiCommon';
import {AddInvestmentDialog, AddVehicleDialog} from '../components/DialogComponents';
import {SidebarInvestment, VehicleListItem} from '../components/SidebarComponents';



import {updateInvestment} from '../api/InvestmentApi';
import {ConfirmDeleteDialog} from '../components/DialogComponents';
import {InvestmentModelGraph} from '../components/GraphComponents';
import {InvestmentFormDefaults, investmentFormSchema} from '../forms/FormSchema';
import {InvestmentDataForm} from '../forms/InvestmentDataForm';
import {convertDates} from '../api/ConvertUtils';
import {ExpensesTable} from '../forms/ExpenseTable';
import {useSnackbar} from 'notistack';
import { Link } from 'react-router-dom'; 

export const PresetContext = React.createContext<ApiPresetData | undefined>(undefined);
 
  
export const InvestmentsPage = () => { 
	// const [vehicleAddInvTarget, setVehicleAddInvTarget] = React.useState<number>(-1); 
	// const [invAddDialogOpen, setInvAddDialogOpen] = React.useState(false);
	const portfolio = useLoaderData() as Portfolio;

	//the portfolio is undefined here- can't figure this out


	//const {investments, investmentVehicles: vehicles} = portfolio;
	const [presetData, setPresetData] = React.useState<ApiPresetData | undefined>(undefined);

	const [invAddDialogOpen, setInvAddDialogOpen] = React.useState(false);
	const [vehicleAddDialogOpen, setVehicleAddDialogOpen] = React.useState(false);
	const [vehicleAddInvTarget, setVehicleAddInvTarget] = React.useState<number>(-1); //by default, adds as individual investment

	React.useEffect(() => {
		if (presetData === undefined) {
			getAnalysisPresets().then((data) => {
				setPresetData(data);
			});
		}
	}, [presetData]);
	

	//const [investments] = React.useState<any[]>([]);
	//const [investments] = [1, 2, 3];
	let investments2: string[] = ['Apple', 'Orange', 'Banana'];

	const openAddInvDialog = (vehicleId: number) => {
		setVehicleAddInvTarget(vehicleId);
		setInvAddDialogOpen(true);
	};

	
	console.log("Investments in investment page");
	console.log(portfolio);

	return <div><PresetContext.Provider value={presetData}><h2>Investments</h2>
		{investments2.map(() => (<h1> bray </h1>))}
		<Button onClick={() => openAddInvDialog(-1)}>
			<Icon baseClassName='material-icons'>add_circle</Icon>
			<Typography variant='body1' component='div' sx={{marginLeft: '10px'}}>
				Add Investment
			</Typography>
		</Button>
		<AddInvestmentDialog
			open={invAddDialogOpen}
			onClose={() => setInvAddDialogOpen(false)}
			vehicleTarget={vehicleAddInvTarget}
		/>
	</PresetContext.Provider>
	</div>;
} 

// export function InvestmentsPage() { 
// 	const [investments] = React.useState<any[]>([]);
// 	return <h2>Investments</h2>
// } 