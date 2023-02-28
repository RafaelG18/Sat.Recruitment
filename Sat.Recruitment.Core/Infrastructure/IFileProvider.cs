using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sat.Recruitment.Core.Infrastructure
{
    /// <summary>
    /// A file provider abstraction
    /// </summary>
    public interface IFileProvider
    {
        /// <summary>
        /// Creates all directories and subdirectories in the specified path unless they already exist
        /// </summary>
        /// <param name="path">The directory to create</param>
        void CreateDirectory(string path);

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk
        /// </summary>
        /// <param name="path">The path to test</param>
        /// <returns>
        /// true if path refers to an existing directory; false if the directory does not exist or an error occurs when
        /// trying to determine if the specified file exists
        /// </returns>
        bool DirectoryExists(string path);

        /// <summary>
        /// Determines whether the specified file exists
        /// </summary>
        /// <param name="filePath">The file to check</param>
        /// <returns>
        /// True if the caller has the required permissions and path contains the name of an existing file; otherwise,
        /// false.
        /// </returns>
        bool FileExists(string filePath);

        /// <summary>
        /// Creates a new file by using the specified encoding, writes a collection of strings
        /// to the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to write to</param>
        /// <param name="contents">The string lines to write to the file</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// </returns>
        Task WriteAllLinesAsync(string path, IEnumerable<string> contents);

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes
        /// the file.
        /// </summary>
        /// <param name="path">The file to open for reading</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains string array containing all lines of the file
        /// </returns>
        Task<string[]> ReadAllLinesAsync(string path);

        /// <summary>
        /// Asynchronously appends lines to a file, and then closes the file. If the specified file does
        /// not exist, this method creates a file, writes the specified lines to the file,
        /// and then closes the file.
        /// </summary>
        /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
        /// <param name="lines">The lines to append to the file.</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// </returns>
        Task AppendAllLinesAsync(string path, IEnumerable<string> lines);

        /// <summary>
        /// Gets or sets the absolute path to the base application content.
        /// </summary>
        string ContentRootPath { get; }
    }
}