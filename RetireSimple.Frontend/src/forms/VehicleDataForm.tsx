import {Box, Grid, Typography} from '@mui/material';
import {useFormContext, useWatch} from 'react-hook-form';
import {FormSelectField, FormTextField} from '../components/InputComponents';
import {Analysis401kForm} from './analysis/Analysis401kForm';
import React from 'react';

export interface VehicleDataFormProps {
	defaultValues?: any;
	disableTypeSelect?: boolean;
	children?: React.ReactNode;
}

export const VehicleDataForm = (props: VehicleDataFormProps) => {
	const formContext = useFormContext();

	const investmentVehicleType = useWatch({
		name: 'investmentVehicleType',
		control: formContext.control,
		defaultValue: '401k',
	});

	const {errors} = formContext.formState;

	//==============================================
	//Field definitions (To reduce indent depth)
	//==============================================

	const vehicleNameField = (
		<FormTextField
			name='investmentVehicleName'
			label='Name'
			control={formContext.control}
			errorField={errors.investmentVehicleName}
			tooltip='The name of this investment vehicle. Usually a personally identifiable name.'
		/>
	);

	const vehicleTypeField = (
		<FormSelectField
			name='investmentVehicleType'
			label='Vehicle Type'
			control={formContext.control}
			errorField={errors.investmentVehicleType}
			defaultOption='401k'
			options={[
				{value: '401k', label: '401k'},
				{value: 'IRA', label: 'IRA'},
				{value: 'RothIRA', label: 'Roth IRA'},
				{value: '403b', label: '403b'},
				{value: '457', label: '457'},
			]}
			disable={props.disableTypeSelect ?? false}
			tooltip='The type of vehicle this is. This does alter how we determine the tax-applied model.'
		/>
	);

	const cashHoldingField = (
		<FormTextField
			name='cashHoldings'
			label='Cash Holdings'
			control={formContext.control}
			errorField={errors.cashHoldings}
			tooltip='The amount of cash in the vehicle that is not invested in a security.'
		/>
	);

	const analysisLengthField = (
		<FormTextField
			name='analysis_analysisLength'
			label='Analysis Length'
			control={formContext.control}
			errorField={errors.analysis_analysisLength}
			tooltip='The number of months starting from today to run the analysis for.'
		/>
	);

	const shortTermCapitalGainsField = (
		<FormTextField
			name='analysis_shortTermCapitalGainsTax'
			label='Capital Gains Tax (Short Term)'
			control={formContext.control}
			errorField={errors.analysis_shortTermCapitalGainsTax}
			tooltip='The tax rate applied to short term capital gains.'
			// decoration='percent'
		/>
	);

	const longTermCapitalGainsField = (
		<FormTextField
			name='analysis_longTermCapitalGainsTax'
			label='Capital Gains Tax (Long Term)'
			control={formContext.control}
			errorField={errors.analysis_longTermCapitalGainsTax}
			tooltip='The tax rate applied to long term capital gains.'
			// decoration='percent'
		/>
	);

	const analysisSubform = React.useMemo(() => {
		switch (investmentVehicleType) {
			case '401k':
				return <Analysis401kForm />;
			default:
				return (
					<Grid item xs={4}>
						<FormTextField
							name='analysis_userContributionFixed'
							label='User Contribution'
							control={formContext.control}
							errorField={errors.analysis_userContributionFixed}
							tooltip='The amount of money that you contribute to this vehicle each month.'
							decoration='currency'
						/>
					</Grid>
				);
		}
	}, [errors.analysis_userContributionFixed, formContext.control, investmentVehicleType]);

	return (
		<>
			<Box>
				<Grid container spacing={2}>
					<Grid item xs={4}>
						{vehicleNameField}
					</Grid>
					<Grid item xs={4}>
						{vehicleTypeField}
					</Grid>
					<Grid item xs={4} />
					<Grid item xs={4}>
						{cashHoldingField}
					</Grid>
					<Grid item xs={8} />
					<Grid item xs={12}>
						<Typography variant='subtitle2'>Analysis Configuration</Typography>
					</Grid>
					<Grid item xs={4}>
						{analysisLengthField}
					</Grid>
					<Grid item xs={4}>
						{shortTermCapitalGainsField}
					</Grid>
					<Grid item xs={4}>
						{longTermCapitalGainsField}
					</Grid>

					{analysisSubform}
				</Grid>
			</Box>
			{props.children}
		</>
	);
};
