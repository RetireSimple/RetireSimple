import {
	Box,
	Button,
	Icon,
	IconButton,
	Paper,
	TableBody,
	TableCell,
	TableContainer,
	TableHead,
	TableRow,
} from '@mui/material';
import React from 'react';
import {ApiExpense} from '../Interfaces';
import {deleteExpense, getExpenses} from '../api/InvestmentApi';
import {AddExpenseDialog} from '../components/DialogComponents';

interface ExpensesTableProps {
	investmentId: number;
}

export const ExpensesTable = (props: ExpensesTableProps) => {
	const [expenses, setExpenses] = React.useState<ApiExpense[]>([]);
	const [addDialogOpen, setAddDialogOpen] = React.useState(false);
	const [needsUpdate, setNeedsUpdate] = React.useState(true);

	React.useEffect(() => {
		if (needsUpdate) {
			getExpenses(props.investmentId).then((data) => {
				setExpenses(data);
				setNeedsUpdate(false);
			});
		}
	}, [needsUpdate, props.investmentId]);

	const handleDelete = (expenseId: number) => {
		deleteExpense(expenseId).then(() => {
			setNeedsUpdate(true);
		});
	};

	return (
		<Box sx={{width: '100%', alignSelf: 'start'}}>
			<Button
				startIcon={<Icon baseClassName='material-icons'>plus</Icon>}
				onClick={() => setAddDialogOpen(true)}>
				Add Expense
			</Button>
			<TableContainer component={Paper}>
				<TableHead>
					<TableRow>
						<TableCell>Amount</TableCell>
						<TableCell>Expense Type</TableCell>
						<TableCell>Expense Date(s)</TableCell>
						<TableCell>Recurrence</TableCell>
						<TableCell></TableCell>
					</TableRow>
				</TableHead>
				<TableBody>
					{expenses.map((expense) => (
						<TableRow key={expense.expenseId}>
							<TableCell>{expense.amount}</TableCell>
							<TableCell>{expense.expenseType}</TableCell>
							<TableCell>
								{expense.expenseType === 'Recurring'
									? `${expense.expenseData['startDate']} - ${expense.expenseData['endDate']}`
									: expense.expenseData['date']}
							</TableCell>
							<TableCell>
								{expense.expenseType === 'OneTime'
									? 'N/A'
									: `${expense.expenseData['frequency']} mos.`}
							</TableCell>
							<TableCell>
								<IconButton onClick={() => handleDelete(expense.expenseId)}>
									<Icon baseClassName='material-icons'>delete</Icon>
								</IconButton>
							</TableCell>
						</TableRow>
					))}
				</TableBody>
			</TableContainer>
			<AddExpenseDialog
				show={addDialogOpen}
				onClose={() => setAddDialogOpen(false)}
				investmentId={props.investmentId}
				setNeedsUpdate={setNeedsUpdate}
			/>
		</Box>
	);
};
