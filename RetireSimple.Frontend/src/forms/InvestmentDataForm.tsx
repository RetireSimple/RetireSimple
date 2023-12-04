import {Box, Grid} from '@mui/material';
import React from 'react';
import {useFormContext, useWatch} from 'react-hook-form';
import {FormSelectField, FormTextField} from '../components/InputComponents';
import {BondForm} from './investment/BondForm';
import {StockForm} from './investment/StockForm';
import {PensionForm} from './investment/PensionForm';
import { Investment } from '../Interfaces';

export interface InvestmentDataFormProps {
	defaultValues?: any;
	disableTypeSelect?: boolean;
	children?: React.ReactNode;
	selectedInvestment?: Investment;
}

export const InvestmentDataForm = (props: InvestmentDataFormProps) => {
	const formContext = useFormContext();

	const investmentType = useWatch({
		name: 'investmentType',
		control: formContext.control,
		defaultValue: 'StockInvestment',
	});
	const {errors} = formContext.formState;

	//==============================================
	//Field definitions (To reduce indent depth)
	//==============================================
	const investmentNameField = (
		<FormTextField
			name='investmentName'
			label='Name'
			//defaultValue={props.selectedInvestment ? props.selectedInvestment.investmentName : ''}
			control={formContext.control}
			errorField={errors.investmentName}
			tooltip='The name of this investment. Can be a personally identifiable name.'
		/>
	);

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
				{value: 'PensionInvestment', label: 'Pension/Social Security'},
			]}
			disable={props.disableTypeSelect ?? false}
			tooltip='The type of security this investment represents.'
		/>
	);

	const investmentTypeSubform = React.useMemo(() => {
		switch (investmentType) {
			case 'StockInvestment':
				return (
					<StockForm
						analysisTypeField={
							<FormSelectField
								name='analysisType'
								label='Analysis Type'
								control={formContext.control}
								errorField={errors.analysisType}
								options={[
									{
										value: 'MonteCarlo',
										label: 'Monte Carlo',
									},
									{
										value: 'BinomialRegression',
										label: 'Binomial Regression',
									},
								]}
								defaultOption=''
								disable={false}
								tooltip='The type of analysis to run on this investment. Only Monte Carlo Simulations are currently supported.'
							/>
						}
					/>
				);
			case 'BondInvestment':
				return (
					<BondForm
						analyisisTypeField={
							<FormSelectField
								name='analysisType'
								label='Analysis Type'
								control={formContext.control}
								errorField={errors.analysisType}
								options={[{value: 'StdBondValuation', label: 'Bond Valuation'}]}
								defaultOption=''
								disable={false}
								tooltip='The type of analysis to run on this investment. Only standard bond valuation is currently supported.'
							/>
						}
					/>
				);
			case 'PensionInvestment':
				return (
					<PensionForm
						analysisTypeField={
							<FormSelectField
								name='analysisType'
								label='Analysis Type'
								control={formContext.control}
								errorField={errors.analysisType}
								options={[
									{
										value: 'PensionSimulation',
										label: 'Pension Simulation',
									},
								]}
								defaultOption=''
								disable={false}
								tooltip='The type of analysis to run on this investment. Only pension simulations are currently supported.'
							/>
						}
					/>
				);
			default:
				return <div>Unknown investment type</div>;
		}
	}, [investmentType, formContext.control, errors]);

	return (
		<>
			<Box>
				<Grid container spacing={2}>
					<Grid item xs={4}>
						{investmentNameField}
					</Grid>
					<Grid item xs={4}>
						{investmentTypeField}
					</Grid>
				</Grid>
				{investmentTypeSubform}
			</Box>
			{props.children}
		</>
	);
};
