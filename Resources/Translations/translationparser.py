import csv, json

file_csv = 'translations.csv'
file_json = 'translations.json'

values = json.load(open(file_json))
output = csv.writer(open(file_csv, 'w', newline = '', encoding = 'utf-8'), delimiter = ';')


def recursive_parse(v, name):
    if isinstance(v, dict):
        for i in v:
            recursive_parse(v[i], name + i)
    elif isinstance(v, list):
        row = [name]
        for i in v:
            row += [i.replace('\n','\\n')]
        output.writerow(row)
    
recursive_parse(values, "")

a = input("JSON Converted to CSV. Press ENTER to save.")

# TODO: Add code for exporting only English, Comment and a language of choice into JSON.