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
  
export function InvestmentPage() { 
	return <h2>Investments</h2>;
} 