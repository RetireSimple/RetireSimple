import {Box, Grid} from '@mui/material';
import React from 'react';
import {useFormContext, useWatch} from 'react-hook-form';
import {FormSelectField, FormTextField} from './Inputs';
import {BondForm} from './investment/BondForm';
import {StockForm} from './investment/StockForm';

export interface InvestmentDataFormProps {
	defaultValues?: any;
	disableTypeSelect?: boolean;
	children?: React.ReactNode;
}

///IMPORTANT CAVEAT: This form does not use a standard submit action
///Data should be validated by calling trigger, then true promise calls getValues()
///Allows for parents to retrieve data from the form context

export const InvestmentDataForm = (props: InvestmentDataFormProps) => {
	const formContext = useFormContext();

	const investmentType = useWatch({
		name: 'investmentType',
		control: formContext.control,
		defaultValue: 'StockInvestment',
	});
	const { errors } = formContext.formState;

	const investmentTypeSubform = React.useMemo(() => {
		switch (investmentType) {
		case 'StockInvestment':
			return <StockForm />;
		case 'BondInvestment':
			return <BondForm />;
		default:
			return <div>Unknown investment type</div>;
		}
	}, [investmentType]);

	//==============================================
	//Field definitions (To reduce indent depth)
	//==============================================
	const investmentNameField = (
		<FormTextField
			name='investmentName'
			label='Name'
			control={formContext.control}
			errorField={errors.investmentName}
		/>);

	const investmentTypeField = (
		<FormSelectField
			name='investmentType'
			label='Investment Type'
			control={formContext.control}
			errorField={errors.investmentType}
			defaultOption='StockInvestment'
			options={[
				{value: 'StockInvestment', label: 'Stock'},
				{value: 'BondInvestment', label: 'Bond'},
			]}
			disable={props.disableTypeSelect ?? false}
		/>);

	return (
		<>
			<Box>
				<Grid container spacing={2}>
					<Grid item xs={4}>{investmentNameField}</Grid>
					<Grid item xs={4}>{investmentTypeField}</Grid>
				</Grid>
				{investmentTypeSubform}
			</Box>
			{props.children}
		</>);
};
