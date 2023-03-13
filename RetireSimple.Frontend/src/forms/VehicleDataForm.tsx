import {Box, Grid} from '@mui/material';
import {useFormContext} from 'react-hook-form';
import {FormSelectField, FormTextField} from './Inputs';

export interface VehicleDataFormProps {
	defaultValues?: any;
	disableTypeSelect?: boolean;
	children?: React.ReactNode;
}

///IMPORTANT CAVEAT: This form does not use a standard submit action
///Data should be validated by calling trigger, then true promise calls getValues()
///Allows for parents to retrieve data from the form context

export const VehicleDataForm = (props: VehicleDataFormProps) => {
	const formContext = useFormContext();

	const {errors} = formContext.formState;

	//==============================================
	//Field definitions (To reduce indent depth)
	//==============================================

	const vehicleNameField = (
		<FormTextField
			name='investmentVehicleName'
			label='Name'
			control={formContext.control}
			errorField={errors.vehicleName}
		/>
	);

	const vehicleTypeField = (
		<FormSelectField
			name='investmentVehicleType'
			label='Vehicle Type'
			control={formContext.control}
			errorField={errors.vehicleType}
			defaultOption='401k'
			options={[
				{value: '401k', label: '401k'},
				{value: 'IRA', label: 'IRA'},
				{value: 'RothIRA', label: 'Roth IRA'},
				{value: '403b', label: '403b'},
				{value: '457', label: '457'},
			]}
			disable={props.disableTypeSelect ?? false}
		/>
	);

	const analysisLengthField = (
		<FormTextField
			name='analysis_analysisLength'
			label='Analysis Length'
			control={formContext.control}
			errorField={errors.analysisLength}
		/>
	);

	const cashContributionField = (
		<FormTextField
			name='analysis_cashContribution'
			label='Cash Contribution'
			control={formContext.control}
			errorField={errors.cashContribution}
		/>
	);

	const vehicleTaxPercentageField = (
		<FormTextField
			name='analysis_vehicleTaxPercentage'
			label='Vehicle Tax Percentage'
			control={formContext.control}
			errorField={errors.vehicleTaxPercentage}
		/>
	);

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
						{analysisLengthField}
					</Grid>
					<Grid item xs={4}>
						{cashContributionField}
					</Grid>
					<Grid item xs={4}>
						{vehicleTaxPercentageField}
					</Grid>
				</Grid>
			</Box>
			{props.children}
		</>
	);
};
