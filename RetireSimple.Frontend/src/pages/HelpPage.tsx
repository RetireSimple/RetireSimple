import {yupResolver} from '@hookform/resolvers/yup';
import {Box, Button, Divider, Tab, Tabs, Typography} from '@mui/material';
import React from 'react';
import {FormProvider, useForm, useFormState} from 'react-hook-form';
import {FieldValues} from 'react-hook-form/dist/types';
import {useFormAction, useLoaderData, useSubmit} from 'react-router-dom';
import {Investment} from '../Interfaces';
import {updateInvestment} from '../api/InvestmentApi';
import {ConfirmDeleteDialog} from '../components/DialogComponents';
import {InvestmentModelGraph} from '../components/GraphComponents';
import {InvestmentFormDefaults, investmentFormSchema} from '../forms/FormSchema';
import {InvestmentDataForm} from '../forms/InvestmentDataForm';
import {convertDates} from '../api/ConvertUtils';
import {ExpensesTable} from '../forms/ExpenseTable';
import {useSnackbar} from 'notistack';
import { Link } from 'react-router-dom'; 
import { getTestData } from '../api/NewApiMapper';
  

function doGetTestData(){
	getTestData()
}

export function HelpPage() { 
	return (<div>
		<h1>Help</h1>
		<Button onClick={doGetTestData}>
			Test
		</Button>
		
		<Box sx={{padding: '2rem'}}>
			<Typography variant='h6' sx={{flexGrow: 1, marginBottom: '0.5rem'}}>
				Welcome to RetireSimple!
			</Typography>
			<Typography variant='body1' sx={{flexGrow: 1, marginBottom: '0.5rem'}}>
				To get started, add some information about investments or vehicles
				to the application.
			</Typography>
			<Typography variant='body1' sx={{flexGrow: 1, marginBottom: '0.5rem'}}>
				Once you have added some information, you can view a portfolio
				analysis by clicking the button below.
			</Typography>
			<Typography variant='body1' sx={{flexGrow: 1, marginBottom: '0.5rem'}}>
				You can come back here using the "Home" item in the top of the
				sidebar list.
			</Typography>
			<br />
			<Typography variant='h6'>TODO: Create help page/ wizard </Typography>
			<br />
		</Box>
	</div>
	);
    
} 