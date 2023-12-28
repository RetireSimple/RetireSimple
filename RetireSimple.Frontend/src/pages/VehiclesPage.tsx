import {yupResolver} from '@hookform/resolvers/yup';
import {Box, Button, Divider, Icon, Tab, Tabs, Typography} from '@mui/material';
import React from 'react';
import {FormProvider, useForm, useFormState} from 'react-hook-form';
import {FieldValues} from 'react-hook-form/dist/types';
import {useFormAction, useLoaderData, useSubmit} from 'react-router-dom';
import {ApiPresetData, Investment, InvestmentModel, InvestmentVehicle, Portfolio} from '../Interfaces';
import {updateInvestment} from '../api/InvestmentApi';
import {AddInvestmentDialog, AddVehicleDialog, ConfirmDeleteDialog} from '../components/DialogComponents';
import {InvestmentModelGraph} from '../components/GraphComponents';
import {InvestmentFormDefaults, investmentFormSchema} from '../forms/FormSchema';
import {InvestmentDataForm} from '../forms/InvestmentDataForm';
import {convertDates} from '../api/ConvertUtils';
import {ExpensesTable} from '../forms/ExpenseTable';
import {useSnackbar} from 'notistack';
import { Link } from 'react-router-dom'; 
import { PresetContext } from '../Layout';
import { InvestmentComponent } from '../components/InvestmentComponent';
import { getAnalysisPresets } from '../api/ApiCommon';
import { VehicleComponent } from '../components/VehicleComponent';
  
export function VehiclesPage() { 

	const portfolio = useLoaderData() as Portfolio;

	//the portfolio is undefined here- can't figure this out
	const {investments, investmentVehicles: vehicles} = portfolio;
	
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
	

	const openAddInvDialog = (vehicleId: number) => {
		setVehicleAddInvTarget(vehicleId);
		setInvAddDialogOpen(true);
	};

	const openEditDialog = () => {
		console.log("PRESS Vehicle FROM FUNC");
		//setEdit(true);
	};


	return <div><PresetContext.Provider value={presetData}><h2>Vehicles</h2>
		{/* {investments.map((investment: Investment) => (<h1> {investment.investmentName} </h1>))} */}
		{vehicles.map((vehicle: InvestmentVehicle) => 
			(VehicleComponent(vehicle, () => {openEditDialog()})))}
		<Button onClick={() => setVehicleAddDialogOpen(true)}>
			<Icon baseClassName='material-icons'>add_circle</Icon>
			<Typography variant='body1' component='div' sx={{marginLeft: '10px'}}>
				Add Vehicle
			</Typography>
		</Button>
		<AddVehicleDialog
			open={vehicleAddDialogOpen}
			onClose={() => setVehicleAddDialogOpen(false)}
		/>
	</PresetContext.Provider>
	</div>;
} 