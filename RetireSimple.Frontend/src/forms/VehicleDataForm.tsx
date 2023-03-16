import {Box, Grid} from '@mui/material';
import {useFormContext} from 'react-hook-form';
import {FormSelectField, FormTextField} from '../components/InputComponents';

export interface VehicleDataFormProps {
	defaultValues?: any;
	disableTypeSelect?: boolean;
	children?: React.ReactNode;
}

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

	const cashContributionField = (
		<FormTextField
			name='analysis_cashContribution'
			label='Contribution Amount (Monthly)'
			control={formContext.control}
			errorField={errors.analysis_cashContribution}
			tooltip='The amount of cash to contribute to the vehicle each month.'
		/>
	);

	const vehicleTaxPercentageField = (
		<FormTextField
			name='analysis_vehicleTaxPercentage'
			label='Vehicle Tax Percentage'
			control={formContext.control}
			errorField={errors.analysis_vehicleTaxPercentage}
			tooltip='The percentage of the vehicle that is taxed. This is used to determine the tax applied model.'
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
						{cashHoldingField}
					</Grid>
					<Grid item xs={8} />
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
