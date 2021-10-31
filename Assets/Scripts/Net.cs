using UnityEngine;

public class Matrix {
	public int rows;
	public int cols;
	public float[,] data; // 2d array of floats
	
	public Matrix(int r, int c) {
		rows = r;
		cols = c;
		data = new float[rows, cols];
	}
	
	public Matrix(float[,] d) {
		rows = d.GetLength(0);
		cols = d.GetLength(1);
		data = d;
	}

	// Operators
	public static Matrix operator +(Matrix a, Matrix b) {
		if (a.rows != b.rows || a.cols != b.cols)
			throw new System.Exception("Matrix dimensions must agree, (" + a.rows + "x" + a.cols + ") != (" + b.rows + "x" + b.cols + ")");
		Matrix c = new Matrix(a.rows, a.cols);
		for (int i = 0; i < a.rows; i++)
			for (int j = 0; j < a.cols; j++)
				c.data[i, j] = a.data[i, j] + b.data[i, j];
		return c;
	}
	public static Matrix operator *(Matrix a, Matrix b) {
		if (a.cols != b.rows)
			throw new System.Exception("Dim 2 of matrix a must equal dim 1 of matrix b, (" + a.rows + "x" + a.cols + ") != (" + b.rows + "x" + b.cols + ")");
		Matrix c = new Matrix(a.rows, b.cols);
		for (int i = 0; i < a.rows; i++)
			for (int j = 0; j < b.cols; j++)
				for (int k = 0; k < a.cols; k++)
					c.data[i, j] += a.data[i, k] * b.data[k, j];
		return c;
	}
	public static Matrix operator *(Matrix a, float b) {
		Matrix c = new Matrix(a.rows, a.cols);
		for (int i = 0; i < a.rows; i++)
			for (int j = 0; j < a.cols; j++)
				c.data[i, j] = a.data[i, j] * b;
		return c;
	}
	public static Matrix operator /(Matrix a, float b) {
		Matrix c = new Matrix(a.rows, a.cols);
		for (int i = 0; i < a.rows; i++)
			for (int j = 0; j < a.cols; j++)
				c.data[i, j] = a.data[i, j] / b;
		return c;
	}
	public static Matrix operator -(Matrix a, Matrix b) => a + (-b);
	public static Matrix operator *(float a, Matrix b) => b * a;
	public static Matrix operator /(float a, Matrix b) => b / a;
	public static Matrix operator -(Matrix a) => a * -1;


	// Methods
	public void Randomize(float min, float max) {
		for (int i = 0; i < rows; i++)
			for (int j = 0; j < cols; j++)
				data[i, j] = Random.Range(min, max);
	}

	public void Mutate(float mutationRate, float mutationChance) {
		for (int i = 0; i < rows; i++)
			for (int j = 0; j < cols; j++)
				if (Random.Range(0f, 1f) < mutationChance)
					data[i, j] += Random.Range(-1f, 1f) * mutationRate;
	}

	public void Print() {
		for (int i = 0; i < rows; i++) {
			string s = "";
			for (int j = 0; j < cols; j++)
				s += data[i, j] + " ";
			Debug.Log(s);
		}
	}
}

public class Netmath {
	// Sigmoid function
	public static float Sigmoid(float z) {
		return 1.0f / (1.0f + Mathf.Exp(-z)); // a
	}
	public static Matrix Sigmoid(Matrix z) {
		Matrix c = new Matrix(z.rows, z.cols);
		for (int i = 0; i < z.rows; i++)
			for (int j = 0; j < z.cols; j++)
				c.data[i, j] = Sigmoid(z.data[i, j]);
		return c; // A
	}

	// Forward propagation
	public static Matrix Forward(Matrix x, Matrix[] w, Matrix[] b) {
		Matrix a = x;
		for (int i = 0; i < w.Length; i++) {
			Matrix z = Netmath.Sigmoid((w[i] * a) + b[i]);
			a = z;
		}
		return a;
	}
}