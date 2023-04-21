import {Box, Grid, Typography} from '@mui/material';
import React from 'react';
import {useFormContext, useWatch} from 'react-hook-form';
import {
	FormDatePicker,
	FormTextFieldCurrency,
	FormTextFieldPercent,
} from '../../components/InputComponents';
import {PensionSimAnalysisForm} from '../analysis/PensionSimAnalysisForm';

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

	const analysisSubForm = React.useCallback(() => {
		switch (analysisType) {
			case 'PensionSimulation':
				return <PensionSimAnalysisForm />;
			default:
				return (
					<Grid item xs={12}>
						<Typography variant='subtitle2'>
							No analysis parameters available
						</Typography>
					</Grid>
				);
		}
	}, [analysisType]);

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
					<Typography variant='subtitle2'>Note about Modeling Social Security</Typography>
				</Grid>
				<Grid item xs={12}>
					<Typography variant='body2'>
						Currently, we treate Social Security Payments to function similarly to that
						of a pension. However, this does mean that we don't provide the direct
						calculation of your estimated Social Security benefits by the time you
						retire.
					</Typography>
					<Typography variant='body2'>
						This is currently documented as a possible feature, but we recommend using
						other soruces (such as the Official Social Security Administration's
						Calculators) to determine your benefits.
					</Typography>
				</Grid>

				<Grid item xs={12}>
					<Typography variant='subtitle2'>Analysis Configuration</Typography>
				</Grid>
				<Grid item xs={4}>
					{props.analysisTypeField}
				</Grid>
				{analysisSubForm()}
			</Grid>
		</Box>
	);
};
