import {Box, Grid, Typography} from '@mui/material';
import React from 'react';
import {useFormContext, useWatch} from 'react-hook-form';
import {
	FormDatePicker,
	FormTextFieldCurrency,
	FormTextFieldMonthUnits,
	FormTextFieldPercent,
} from '../../components/InputComponents';

export interface PensionFormProps {
	defaultValues?: any;
	analysisTypeField: React.ReactNode;
}

export const PensionForm = (props: PensionFormProps) => {
	const formContext = useFormContext();
	const analysisType = useWatch({
		name: 'analysisType',
		control: formContext.control,
		defaultValue: props.defaultValues?.analysisType ?? 'PensionSimulation',
	});

	const {errors} = formContext.formState;

	const expectedTaxRateField = React.useMemo(
		() => (
			<FormTextFieldPercent
				name='expectedTaxRate'
				label='Expected Tax Rate'
				control={formContext.control}
				errorField={errors.expectedTaxRate}
				tooltip={
					<>
						<Typography variant='inherit'>
							The expected tax rate on the pension payments.
						</Typography>
					</>
				}
			/>
		),
		[formContext.control, errors.expectedTaxRate],
	);

	const analysisSubForm = React.useCallback(() => {
		switch (analysisType) {
			case 'PensionSimulation':
				return (
					<>
						<Grid item xs={4}>
							{expectedTaxRateField}
						</Grid>
					</>
				);
			default:
				return (
					<Grid item xs={12}>
						<Typography variant='subtitle2'>
							No analysis parameters available
						</Typography>
					</Grid>
				);
		}
	}, [analysisType, expectedTaxRateField]);

	//==============================================
	//Field definitions (To reduce indent depth)
	//==============================================
	const pensionInitialMonthlyPaymentField = (
		<FormTextFieldCurrency
			name='pensionInitialMonthlyPayment'
			label='Initial Monthly Payment'
			control={formContext.control}
			errorField={errors.pensionInitialMonthlyPayment}
			tooltip={
				<>
					<Typography variant='inherit'>
						The initial monthly payment from the pension.
					</Typography>
					<Typography variant='inherit'>
						This is the amount that will be paid out each month from the pension.
					</Typography>
				</>
			}
		/>
	);

	const pensionYearlyIncreaseField = (
		<FormTextFieldPercent
			name='pensionYearlyIncrease'
			label='Yearly Increase'
			control={formContext.control}
			errorField={errors.pensionYearlyIncrease}
			tooltip={
				<>
					<Typography variant='inherit'>
						The yearly increase in the pension payment.
					</Typography>
					<Typography variant='inherit'>
						This is the yearly increase in the pension payment.
					</Typography>
				</>
			}
		/>
	);

	const pensionStartDateField = (
		<FormDatePicker
			name='pensionStartDate'
			label='Start Date'
			control={formContext.control}
			errorField={errors.pensionStartDate}
			tooltip={
				<>
					<Typography variant='inherit'>
						The date that the pension payments will start.
					</Typography>
				</>
			}
			defaultValue={props.defaultValues?.pensionStartDate}
		/>
	);

	const analysisLengthField = (
		<FormTextFieldMonthUnits
			name='analysis_analysisLength'
			label='Analysis Length'
			control={formContext.control}
			errorField={errors.analysis_analysisLength}
			tooltip={
				<>
					<Typography variant='inherit'>
						The length of time to run the analysis for.
					</Typography>
				</>
			}
		/>
	);

	return (
		<Box sx={{flexGrow: 1, marginTop: '1rem'}}>
			<Grid container spacing={2}>
				<Grid item xs={12}>
					<Typography variant='h6'>Pension Information</Typography>
				</Grid>
				<Grid item xs={4}>
					{pensionInitialMonthlyPaymentField}
				</Grid>
				<Grid item xs={4}>
					{pensionYearlyIncreaseField}
				</Grid>
				<Grid item xs={4}>
					{pensionStartDateField}
				</Grid>
				<Grid item xs={12}>
					<Typography variant='subtitle2'>Analysis Configuration</Typography>
				</Grid>
				<Grid item xs={4}>
					{props.analysisTypeField}
				</Grid>
				<Grid item xs={4}>
					{analysisLengthField}
				</Grid>
				{analysisSubForm()}
			</Grid>
		</Box>
	);
};
