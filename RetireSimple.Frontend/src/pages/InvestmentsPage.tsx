
import {Button, Typography, Icon} from '@mui/material';
import React from 'react';

import {useLoaderData} from 'react-router-dom';
import {Investment, Portfolio, ApiPresetData} from '../Interfaces';
import {getAnalysisPresets} from '../api/ApiCommon';
import {AddInvestmentDialog, EditInvestmentDialog} from '../components/DialogComponents';



import { InvestmentComponent } from '../components/InvestmentComponent';

export const PresetContext = React.createContext<ApiPresetData | undefined>(undefined);
 
  
export const InvestmentsPage = () => { 
	// const [vehicleAddInvTarget, setVehicleAddInvTarget] = React.useState<number>(-1); 
	// const [invAddDialogOpen, setInvAddDialogOpen] = React.useState(false);
	const portfolio = useLoaderData() as Portfolio;

	//the portfolio is undefined here- can't figure this out
	const {investments, investmentVehicles: vehicles} = portfolio;
	console.log("INVESTMENTS ");
	console.log(investments);
	const [presetData, setPresetData] = React.useState<ApiPresetData | undefined>(undefined);

	const [editingInvestment, setEditingInvestment] = React.useState<Investment | null>(null);

	const [invAddDialogOpen, setInvAddDialogOpen] = React.useState(false);
	const [editInvestmentDialogOpen, setEditInvestmentDialogOpen] = React.useState(false);
	const [vehicleAddInvTarget, setVehicleAddInvTarget] = React.useState<number>(-1); //by default, adds as individual investment

	React.useEffect(() => {
		if (presetData === undefined) {
			getAnalysisPresets().then((data) => {
				setPresetData(data);
			});
		}
	}, [presetData]);

	const openAddInvDialog = (vehicleId: number) => {
		setVehicleAddInvTarget(vehicleId);
		setInvAddDialogOpen(true);
	};

	const openEditDialog = (investment: Investment) => {
		console.log("PRESS INVESTMENT FROM FUNC");
		setEditingInvestment(investment)
		setEditInvestmentDialogOpen(true);
	};

	return <div><PresetContext.Provider value={presetData}><h2>Investments</h2>
		<Button 
			onClick={() => openAddInvDialog(-1)}
		>
			<Icon baseClassName='material-icons'>add_circle</Icon>
			<Typography variant='body1' component='div' sx={{marginLeft: '10px'}}>
				Add Investment
			</Typography>
		</Button>
		{/* {investments.map((investment: Investment) => (<h1> {investment.investmentName} </h1>))} */}
		{investments.map((investment: Investment) => 
			(InvestmentComponent(investment, () => {openEditDialog(investment)})))}
		<Button 
			onClick={() => openAddInvDialog(-1)}

		>
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
		<EditInvestmentDialog
			open={editInvestmentDialogOpen}
			onClose={() => setEditInvestmentDialogOpen(false)}
			investment={editingInvestment!}
		/>
	</PresetContext.Provider>
	</div>;
} 
