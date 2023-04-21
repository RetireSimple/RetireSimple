import {useFormContext} from 'react-hook-form';
import {FormTextFieldMonthUnits, FormTextFieldPercent} from '../../components/InputComponents';
import {Grid, Typography} from '@mui/material';

export const PensionSimAnalysisForm = () => {
	const formContext = useFormContext();
	const {errors} = formContext.formState;

	//==============================================
	//Field definitions (To reduce indent depth)
	//==============================================
	const analysisLengthField = (
		<FormTextFieldMonthUnits
			name='analysis_analysisLength'
			label='Analysis Length'
			control={formContext.control}
			errorField={errors.analysis_analysisLength}
			tooltip='The number of months starting from today to run the analysis for.'
		/>
	);

	const expectedTaxRateField = (
		<FormTextFieldPercent
			name='analysis_expectedTaxRate'
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
	);

	return (
		<>
			<Grid item xs={4}>
				{analysisLengthField}
			</Grid>
			<Grid item xs={4}>
				{expectedTaxRateField}
			</Grid>
		</>
	);
};
