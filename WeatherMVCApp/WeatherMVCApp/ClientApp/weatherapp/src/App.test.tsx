import { render, screen } from '@testing-library/react';
import App from './App';

test('renders Weather Dashboard', () => {
  render(<App />);
  const linkElement = screen.getByText(/Weather Dashboard/i);
  expect(linkElement).toBeInTheDocument();
});
