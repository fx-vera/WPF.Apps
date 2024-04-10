using PropertyChanged;
using System.Collections.ObjectModel;

namespace Tucodev.ComicLand.Marvel.Models
{
    /// <summary>
    /// Comics data
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class DisplayerModel
    {
        public DisplayerModel() { }

        public ObservableCollection<CreatorComicModel> CreatorComics { get; set; } = new ObservableCollection<CreatorComicModel>();
        public CreatorComicModel SelectedCreator { get; set; }
    }
}