import {yupResolver} from '@hookform/resolvers/yup';
import {Box, Button, Dialog, DialogActions, DialogTitle} from '@mui/material';
import {FieldValues, FormProvider, useForm} from 'react-hook-form';
import {useFormAction, useSubmit} from 'react-router-dom';
import {addInvestment} from '../../api/InvestmentApi';
import {investmentFormSchema} from '../../data/FormSchema';
import {InvestmentDataForm} from '../../forms/InvestmentDataForm';

export interface AddInvestmentDialogProps {
	open: boolean;
	onClose: () => void;
	// vehicleTarget: React.Dispatch<React.SetStateAction<number>>;
}

export const AddInvestmentDialog = (props: AddInvestmentDialogProps) => {
	const formContext = useForm({
		shouldUnregister: true,
		resolver: yupResolver(investmentFormSchema),

	});

	const submit = useSubmit();
	const addAction = useFormAction('/add');

	const handleAdd = (data: FieldValues) => {
		const requestData: {[key: string]: string;} = {};

		Object.entries(data)
			.map(([key, value]) => [key, value.toString()])
			.forEach(([key, value]) => requestData[key] = value);

		addInvestment(requestData).then(() => {
			props.onClose();
			submit(null, {method: 'post', action: addAction});
		});
	};

	return (
		<FormProvider {...formContext}>
			<Dialog open={props.open}
				maxWidth='md'>
				<DialogTitle>Add Investment</DialogTitle>
				<Box sx={{padding: '2rem'}}>
					<InvestmentDataForm>
						<DialogActions>
							<Button onClick={props.onClose}>Cancel</Button>
							<Button onClick={formContext.handleSubmit(handleAdd)}>Add</Button>
						</DialogActions>
					</InvestmentDataForm>
				</Box>
			</Dialog>
		</FormProvider>
	);
};

